// <copyright file="SigninSampleScript.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations

using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Google;
using UnityEngine;

public class FirebaseManager
{
    private string webClientId = "839520731329-9t0mcgm87ljmc8ham68p36eijk5713m7.apps.googleusercontent.com";

    public FirebaseAuth auth;
    public DatabaseReference dbReference;
    private GoogleSignInConfiguration configuration;

    public bool GoogleLogIn { get; private set; } = false;
    public bool CheckFirebaseDone { get; private set; } = false;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    public void Init()
    {
        GoogleLogIn = false;
        CheckFirebaseDone = false;

        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(dependencyTask =>
        {
            if (dependencyTask.IsCanceled || dependencyTask.IsFaulted)
            {
                Debug.LogError("Firebase Initialize Failed: " + dependencyTask.Exception);
                return;
            }

            if (dependencyTask.Result != DependencyStatus.Available)
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyTask.Result);
                return;
            }

            // Firebase 사용 가능
            auth = FirebaseAuth.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            Managers.Data.FirebaseDataInit();

            // 현재 로그인된 유저가 있는 경우만 확인
            var user = auth.CurrentUser;
            if (user == null)
            {
                GoogleLogIn = false;
                CheckFirebaseDone = true;
                Debug.Log("No user is currently logged in.");
                return;
            }

            string uid = user.UserId;
            dbReference.Child("users").Child(uid).GetValueAsync().ContinueWith(userTask =>
            {
                if (userTask.IsFaulted || !userTask.IsCompleted)
                {
                    Debug.LogError("Failed to check user data in database.");
                    return;
                }

                if (userTask.Result.Exists)
                {
                    GoogleLogIn = true;
                    Debug.Log("User data exists in database.");
                }
                else
                {
                    auth.SignOut();
                    GoogleSignIn.DefaultInstance.SignOut();
                    GoogleLogIn = false;
                    Debug.LogWarning("User data not found. Logged out.");
                }

                CheckFirebaseDone = true;
            });
        });
    }

    public void OnSignIn()
    {
        Debug.Log("Calling SignIn");

        // 이미 로그인된 경우 함수 종료
        if (auth.CurrentUser != null)
        {
            Debug.Log("Already signed in as: " + auth.CurrentUser.DisplayName);
            return; 
        }

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        if (GoogleSignIn.DefaultInstance == null)
        {
            Debug.LogError("GoogleSignIn.DefaultInstance is null. Check GoogleSignIn package initialization.");
            return;
        }

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void OnSignOut()
    {
        Debug.Log("Calling SignOut");

        // 로그인된 사용자가 없으면 종료
        if (auth.CurrentUser == null)
        {
            Debug.Log("No user is signed in.");
            return;
        }

        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
        GoogleLogIn = false;
        Managers.Scene.LoadScene(Define.Scene.GamePlay);
    }

    public void OnDisconnect()
    {
        Debug.Log("Calling Disconnect");

        // 로그인된 사용자가 없으면 종료
        if (auth.CurrentUser == null)
        {
            Debug.Log("No user is signed in.");
            return;
        }

        GoogleSignIn.DefaultInstance.Disconnect();
        auth.SignOut();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        if (string.IsNullOrEmpty(idToken))
        {
            Debug.LogError("Google Sign-In failed: ID Token is null or empty.");
            return;
        }

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Sign-in Failed: " + task.Exception?.ToString());
                return;
            }

            Debug.Log("Sign In Successful.");
            GoogleLogIn = true;

            // 인증 완료 후 CurrentUser 확인
            if (auth.CurrentUser != null)
            {
                Debug.Log("User is signed in: " + auth.CurrentUser.DisplayName);
                Managers.Scene.LoadScene(Define.Scene.GamePlay);
            }
            else
            {
                Debug.LogError("CurrentUser is null. Something went wrong.");
            }
        });
    }

    public void OnSignInSilently()
    {
        Debug.Log("Calling SignIn Silently");

        // 이미 로그인된 경우 함수 종료
        if (auth.CurrentUser != null)
        {
            Debug.Log("Already signed in as: " + auth.CurrentUser.DisplayName);
            return;
        }

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        Debug.Log("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
    }
}

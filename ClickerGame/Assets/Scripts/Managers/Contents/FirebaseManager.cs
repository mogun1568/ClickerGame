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

    public bool IsLogIn { get; private set; } = false;
    public bool CheckFirebaseDone { get; private set; } = false;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    public void Init()
    {
        IsLogIn = false;
        CheckFirebaseDone = false;

        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Firebase Initialize Failed: " + task.Exception?.ToString());
                return;
            }

            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Managers.Data.FirebaseDataInit();
                if (auth.CurrentUser != null) IsLogIn = true;
                CheckFirebaseDone = true;

                Debug.Log("Firebase is initialized successfully.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
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
        IsLogIn = false;
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
            IsLogIn = true;

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

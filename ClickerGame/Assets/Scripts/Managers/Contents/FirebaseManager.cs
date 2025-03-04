// <copyright file="SigninSampleScript.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Google;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseManager
{
    private string webClientId = "839520731329-9t0mcgm87ljmc8ham68p36eijk5713m7.apps.googleusercontent.com";

    public FirebaseAuth auth;
    public DatabaseReference dbReference;
    private GoogleSignInConfiguration configuration;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    public void Init()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                //AddToInformation("Firebase Initialize Failed: " + task.Exception?.ToString());
                return;
            }

            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Managers.Data.FirebaseData.Init();

                //AddToInformation("Firebase is initialized successfully.");
                Debug.Log("Firebase is initialized successfully.");
            }
            else
            {
                //AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
        });
    }


    public void OnSignIn()
    {
        //AddToInformation("Calling SignIn");

        if (auth.CurrentUser != null)
        {
            //AddToInformation("Already signed in as: " + auth.CurrentUser.DisplayName);
            return;  // 이미 로그인된 경우 함수 종료
        }

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void OnSignOut()
    {
        //AddToInformation("Calling SignOut");

        if (auth.CurrentUser == null)
        {
            //AddToInformation("No user is signed in.");
            return; // 로그인된 사용자가 없으면 종료
        }

        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
    }

    // 안됨
    public void OnDisconnect()
    {
        //AddToInformation("Calling Disconnect");

        if (auth.CurrentUser == null)
        {
            //AddToInformation("No user is signed in.");
            return; // 로그인된 사용자가 없으면 종료
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
                    //AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    //AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            //AddToInformation("Canceled");
        }
        else
        {
            //AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                //AddToInformation("Sign-in Failed: " + task.Exception?.ToString());
                return;
            }

            //AddToInformation("Sign In Successful.");

            // 인증 완료 후 CurrentUser 확인
            if (auth.CurrentUser != null)
            {
                //AddToInformation("User is signed in: " + auth.CurrentUser.DisplayName);
                //Managers.FirebaseData.LoadGameData();  // 사용자 데이터 로드
            }
            else
            {
                //AddToInformation("CurrentUser is null. Something went wrong.");
            }
        });
    }

    public void OnSignInSilently()
    {
        //AddToInformation("Calling SignIn Silently");

        if (auth.CurrentUser != null)
        {
            //AddToInformation("Already signed in as: " + auth.CurrentUser.DisplayName);
            return;  // 이미 로그인된 경우 함수 종료
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

        //AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
    }
}
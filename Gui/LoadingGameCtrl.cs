﻿using UnityEngine;
using System.Collections;

public class LoadingGameCtrl : MonoBehaviour {
	public UITexture LoadTextTextureCom;
	public UITexture LoadImgTextureCom;
	public Texture[] LoadText;
	public Texture[] LoadImg;

	// Use this for initialization
	void Start()
	{
		IsHiddenLoadingGame = false;
		int loadLevel = Application.loadedLevel - 1;
		if (XkGameCtrl.GetInstance().IsCartoonShootTest) {
			if (loadLevel < 1 || loadLevel > 6) {
				loadLevel = 1;
			}
		}

		LoadTextTextureCom.mainTexture = LoadText[loadLevel];
		LoadImgTextureCom.mainTexture = LoadImg[loadLevel];
		HiddenLoadingGame();
//		float timeVal = 0f;
//		if (!XkGameCtrl.GetInstance().IsCartoonShootTest) {
//			timeVal = 6f;
//		}
//		Invoke("HiddenLoadingGame", timeVal);
	}

	public static int PlayerCount;
	public static void ResetLoadingInfo()
	{
		PlayerCount = 0;
	}

	public static void AddLoadingPlayerCount()
	{
		PlayerCount++;
	}

	static bool IsHiddenLoadingGame;
	public static void SetIsHiddenLoadingGame()
	{
		IsHiddenLoadingGame = true;
	}

	void HiddenLoadingGame()
	{
		if (Network.peerType == NetworkPeerType.Disconnected) {
			gameObject.SetActive(false);
			//XkPlayerCtrl.GetInstanceCartoon().DelayMoveCartoonCamera();
			XKGlobalData.GetInstance().PlayGuanKaBeiJingAudio();
		}
//		else {
//			StartCoroutine(CheckPlayerCountLoop());
//			if (Network.peerType == NetworkPeerType.Client) {
//				NetCtrl.GetInstance().HandleLoadingGamePlayerCount();
//			}
//		}
	}

	IEnumerator CheckPlayerCountLoop()
	{
		bool isStopCheck = false;
		do {
			if (Network.peerType == NetworkPeerType.Server) {
				if (PlayerCount >= Network.connections.Length) {
					isStopCheck = true;
					gameObject.SetActive(false);
					NetCtrl.GetInstance().HandleLoadingGameHiddenLoadingGame();
					XkPlayerCtrl.GetInstanceCartoon().DelayMoveCartoonCamera();
					XKGlobalData.GetInstance().PlayGuanKaBeiJingAudio();
					yield break;
				}
			}
			else {
				if (IsHiddenLoadingGame) {
					isStopCheck = true;
					gameObject.SetActive(false);
					XkPlayerCtrl.GetInstanceCartoon().DelayMoveCartoonCamera();
					XKGlobalData.GetInstance().PlayGuanKaBeiJingAudio();
					yield break;
				}
			}
			yield return new WaitForSeconds(0.1f);
		} while (!isStopCheck);
	}
}
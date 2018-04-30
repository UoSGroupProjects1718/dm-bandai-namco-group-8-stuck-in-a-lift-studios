using UnityEngine;
using System;
using System.Collections.Generic;

// You call it like this : WaveExploPostProcessing.Get().StartIt(myVector2Position);

public class WaveExploPostProcessing : MonoBehaviour {
	public Material mat;
	static WaveExploPostProcessing _postProcessing;

	static public WaveExploPostProcessing Get() {
		WaveExploPostProcessing postProcessing = Camera.main.gameObject.AddComponent<WaveExploPostProcessing>(); 
		return postProcessing;
	}

	public WaveExploPostProcessing() {
//		mat = new Material(Shader.Find("Custom/WaveExplo"));
	}

	public void Awake(){
		mat = new Material(Shader.Find("Custom/WaveExplo"));
	}

	protected float _radius;
	public float radius {
		get { return _radius; }
		set { 
			_radius = value;
			mat.SetFloat("_Radius",_radius);
		}
	}
	public void StartIt(Vector2 center) {
		mat.SetFloat("_CenterX",(center.x + (Screen.width*0.5f))/Screen.width);
		mat.SetFloat("_CenterY",(center.y + (Screen.height*0.5f))/Screen.height);
		radius = 0.1f;

//		TweenConfig config=new TweenConfig().floatProp("radius", 1.34f);
		TweenConfig config=new TweenConfig().floatProp("radius", 1.5f);
		//config.easeType=GoEaseType.ExpoOut;
		config.onComplete(HandleComplete);
//		Go.to(this,0.6f,config);
		Go.to(this,1f,config);
	}
	protected void HandleComplete(AbstractTween tween) {
		Destroy(this);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, mat, -1);
	}
}

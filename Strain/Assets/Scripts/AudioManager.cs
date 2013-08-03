using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : Singleton<AudioManager> 
 {
	class ClipInfo
    {
       //ClipInfo used to maintain default audio source info
       public AudioSource source { get; set; }
       public float defaultVolume { get; set; }
    }
 
    private List<ClipInfo> m_activeAudio;
	public AudioClip[] songArray;
	public AudioClip[] sfxArray;
	public float[] clipVolume;
	public int currentClip = 0;
	public AudioSource msource;
	
    public static AudioManager instance{ get; private set; }
	
     
    void Awake()
		
    {
	m_activeAudio = new List<ClipInfo>();
    if (instance != null && instance != this)
    {
    Destroy( this.gameObject );
    return;
    }
    else
    {
    instance = this;
				
    	DontDestroyOnLoad(gameObject);
		}
    }
     
    public static AudioManager GetInstance()
    {
    return instance;
    }
	private void setSource(ref AudioSource source, AudioClip clip, float volume) {
   source.rolloffMode = AudioRolloffMode.Logarithmic;
   source.dopplerLevel = 0.2f;
   source.minDistance = 150;
   source.maxDistance = 1500;
   source.clip = clip;
   source.volume = volume;
   
	}
	
	
	public AudioSource Play(AudioClip clip, Vector3 soundOrigin, float volume) 
	{
   //Create an empty game object
   GameObject soundLoc = new GameObject("Audio:"+clip.name);
   soundLoc.transform.position = soundOrigin;
	
 
   //Create the source
   AudioSource source = soundLoc.AddComponent<AudioSource>();
   setSource(ref source, clip, volume);
   source.Play();
   Destroy(soundLoc, clip.length);
    
   //Set the source as active
   m_activeAudio.Add(new ClipInfo{source = source, defaultVolume = volume});
   return source;
	
		
	}
	public void setSFXVolume(int sfxvolume)
	{
		currentClip = sfxvolume;
		audio.clip = sfxArray[currentClip];

    	audio.volume = clipVolume[currentClip];
	}
	public AudioSource BGMPlay(AudioClip clip, Vector3 soundOrigin, float volume)
	{
		 //Create an empty game object
   GameObject musicLoc = new GameObject("BGM:"+clip.name);
   musicLoc.transform.position = soundOrigin;
	
 
   //Create the source
   msource = musicLoc.AddComponent<AudioSource>();
   setSource(ref msource, clip, volume);
   msource.Play();
   msource.loop = true;
   DontDestroyOnLoad(musicLoc);
    
   //Set the source as active
   m_activeAudio.Add(new ClipInfo{source = msource, defaultVolume = volume});
   return msource;
	
		
	}
	public void stopSound(AudioSource toStop) {
    try {
        Destroy(m_activeAudio.Find(s => s.source == toStop).source.gameObject);
    } catch {
        Debug.Log("Error trying to stop audio source "+toStop);
    }
}
 
}
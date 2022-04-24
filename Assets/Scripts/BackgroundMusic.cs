using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public Sprite unpauseSpr, pauseSpr;
    public Image pausebotonImage;
    // Start is called before the first frame update
    /* private void Awake()
     {

         GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
         if (objs.Length > 1)
         {
             Destroy(this.gameObject);
         }
         DontDestroyOnLoad(this.gameObject);



     }
     */
    private void Start()
    {
        
    }
    // Update is called once per frame

    public void NoMusic()
    { 
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pausebotonImage.sprite = unpauseSpr;
        }
            
        else
        {
            audioSource.UnPause();
            pausebotonImage.sprite = pauseSpr;
        }
            
    }
   public IEnumerator ComboBackground(int combo)
    {

       if(combo > 2 || combo <= 0)
        {
           
            float volume = ((combo / 6f) * 0.3f) + 0.4f;
            float pitch = ((combo / 6f) * 0.2f) + 0.8f;

            float deltaV = volume - audioSource.volume;
            float deltaP = pitch - audioSource.pitch;
            for (int i = 0; i < 10; i++)
            {
                audioSource.volume += deltaV / 10f;
                audioSource.pitch += deltaP / 10f;
                yield return new WaitForSeconds(.05f);
            }
        }
        
        
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideoMonitor : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private loadScene loader;

    public float waitTime = 1f;
    public int loadLevelIndex;

    private long videoLengthInFrames;

    private void Start()
    {
        loader = this.GetComponent<loadScene>();
        videoLengthInFrames = (long)videoPlayer.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(videoPlayer.frame >= videoLengthInFrames)
        {
            StartCoroutine(waitAndLoad());
        }
    }

    private IEnumerator waitAndLoad()
    {
        yield return new WaitForSeconds(waitTime);
        loader.Load(loadLevelIndex);

    }

    /// <summary>
    /// Sets the Video playback speed. 1 being normal playback, 2 being 2 times speed, 0 being paused, and 10 being max speed
    /// </summary>
    /// <param name="speed"></param>
    public void SetVideoSpeed(float speed)
    {
        videoPlayer.playbackSpeed = speed;
    }
}

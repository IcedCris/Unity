using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
 
    [Header("Main Menu")]
    [SerializeField]
    private GameObject _mainMenu;

    [SerializeField]
    private GameObject _player;

    [Space(20)]
    [Header("Settings Menu")]
    [SerializeField]
    private GameObject _settingsMenu;

    [SerializeField]
    private AudioSource _MusicSource;

    [SerializeField]
    private Image _sound;

    [SerializeField]
    private Image _noSound;

    [SerializeField]
    private Image _music;

    [SerializeField]
    private Image _noMusic;

    
    [Space(20)]
    [Header("InGame")]
    [SerializeField]
    private GameObject _inGame;

    [SerializeField]
    private Text _txtGold;

    [SerializeField]
    private Text _txtBestDistance;

    [SerializeField]
    private Text _txtCurrentDistance;

    [SerializeField]
    private Image _BlackImg;

    private PlayerController _ctrlPLayer;

    private float _reloadTime;
    private bool _end;
    void Awake()
    {
        if (!_player)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        _ctrlPLayer = _player.GetComponent<PlayerController>();
        _ctrlPLayer.run = false;

        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _inGame.SetActive(false);

        _BlackImg.gameObject.SetActive(true);

        StartCoroutine(Fade(true, _BlackImg, 0.5f));
        AjustAudio(PlayerPrefs.GetInt("Sound"));
        AjustMusic(PlayerPrefs.GetInt("Music"));
      
    }

    
    void Update()
    {

        //INGAME:

        if (_ctrlPLayer.dead)
        {
            if (_reloadTime > 10 && !_end)
            {
                _end = true;
                string name = SceneManager.GetActiveScene().name;
                StartCoroutine(Fade(false, _BlackImg, 1f));
                StartCoroutine(Load(name, 1.2f));
                
            }
            else
            {
                _reloadTime += Time.deltaTime;
            }
        }
        else
        {
            _txtCurrentDistance.text = Mathf.RoundToInt(_player.transform.position.z).ToString();
        }
        
        _txtGold.text = Gold.total.ToString();

    
    }

    public void Play()
    {
        _txtBestDistance.text = PlayerPrefs.GetInt("BestDistance").ToString();
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _inGame.SetActive(true);
        _ctrlPLayer.run = true;

    }

    public void SettingsShow()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _inGame.SetActive(false);
    }

    public void SettingsClose()
    {
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _inGame.SetActive(false);
    }

    public void SettingsSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }

        AjustAudio(PlayerPrefs.GetInt("Sound"));
    }

    private void AjustAudio(int vol)
    {
        AudioListener.volume = vol;

        if (vol == 1)
        {
            _noSound.gameObject.SetActive(false);
            _sound.gameObject.SetActive(true);
        }
        else
        {
            _noSound.gameObject.SetActive(true);
            _sound.gameObject.SetActive(false);
        }
    }

    public void SettingsMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }

        AjustMusic(PlayerPrefs.GetInt("Music"));
    }

    private void AjustMusic(int vol)
    {
        if (_MusicSource)
        {
            _MusicSource.volume = PlayerPrefs.GetInt("Music");
        }

        if (vol == 1)
        {
            _noMusic.gameObject.SetActive(false);
            _music.gameObject.SetActive(true);
        }
        else
        {
            _noMusic.gameObject.SetActive(true);
            _music.gameObject.SetActive(false);
        }
       
    }

    IEnumerator Fade(bool In, Image img, float duration)
    {
        float startTime = Time.time;
        float timePassed = 0.0f;
        Color endColor;

        if (In)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
            endColor = new Color(img.color.r, img.color.g, img.color.b, 0.0f);
        }
        else
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.0f);
            endColor = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
        }

        while (timePassed < duration || img.color.a != endColor.a)
        {
            timePassed = Time.time - startTime;
            float nT = Mathf.Clamp(timePassed / duration, 0, 1);
            img.color = Color.Lerp(img.color, endColor, nT);
            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator Load(string name, float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(name);
    }

}

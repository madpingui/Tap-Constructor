using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class StackManager : MonoBehaviour
{
    public Text scoreText;
    public Color32[] gameColors = new Color32[10];
    public Material stackMat;
    public GameObject endPanel,rubble, skyParticle;
    public Mesh[] building;

    private const float BOUNDS_SIZE = 1f;
    private const float STACK_MOVING_SPEED = 5.0f;
    private const float ERROR_MARGIN = 0.1f;
    private const float STACK_BOUNDS_GAIN = 0.25f;
    private const int COMBO_START_GAIN = 4;

    private GameObject[] theStack;
    public Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);
    
    private int stackIndex;
    private int scoreCount = 0;
    private int combo = 0;
    public Text comboNumberText;
    public GameObject PanelCombo;

    private float tileTransition = 0.0f;
    private float tileSpeed = 2.5f;
    private float secundaryPosition;

    private bool isMovingOnX = true;
    private bool gameOver = false;
    private int var;
    private Vector3 desirePosition;
    private Vector3 lastTilePosition;

    private float speedBoost = 1;

    public Image background;
    public Gradient gradientBackground;

    public AudioClip chopClip, comboClip;
    private AudioSource audioSource;
  

    public BackgroundMusic backgroundMusic;
    public GameObject title;
    public Transform city, highScoreObj;
    private Transform offsetCity;

    private void Start()
    {
        offsetCity = city.transform;
        highScoreObj.position = new Vector3(0 ,( PlayerPrefs.GetInt("hiscore") * 0.3f ) +0.6f ,0) ;

        audioSource = gameObject.GetComponent<AudioSource>();
        theStack = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i).gameObject;

            ColorMesh(theStack[i].GetComponent<MeshFilter>().mesh);
        }

        stackIndex = transform.childCount - 1;

        //StartCoroutine("ParticleActivator");
    }

    private void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go =Instantiate(rubble,pos, new Quaternion(0,0,0,0)) ;
 
        go.transform.localScale = scale;
        go.GetComponent<MeshFilter>().mesh = building[theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().meshType];
        go.GetComponent<ResizeTexture>().ResizeTextureUV(scale.z, scale.x);
 
    }

    private void Update()
    {
        if (gameOver)
            return;

        MoveTile();

        transform.position = Vector3.Lerp(transform.position, desirePosition, STACK_MOVING_SPEED * Time.deltaTime);

        city.position = Vector3.Lerp(city.position, new Vector3(offsetCity.position.x, desirePosition.y - 2.5f, offsetCity.position.z), STACK_MOVING_SPEED * Time.deltaTime);

    }

    public void actionButton()
    {
        if (gameOver)
            return;

        if (CheckDead())
        {
            PlaceTile();
            speedBoost += 0.01f;
        }
        else
        {
            EndGame();
        }
        title.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    private void MoveTile()
    {

        tileTransition += Time.deltaTime * tileSpeed;
        if(isMovingOnX)
            theStack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition * speedBoost) * BOUNDS_SIZE, scoreCount *0.3f, secundaryPosition);
        else
            theStack[stackIndex].transform.localPosition = new Vector3(secundaryPosition, scoreCount * 0.3f, Mathf.Sin(tileTransition * speedBoost) * BOUNDS_SIZE);

        if (stackBounds.x <= 0.3f || stackBounds.y <= 0.3f )
        {
            theStack[stackIndex].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[2];
            theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().meshType = 2;
        }
    }

    private void SpawnTile()
    {
        if(theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().meshType != 2)
        {
            theStack[stackIndex].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[1];
            theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().meshType = 1;
            if (stackIndex == transform.childCount - 1)
            {
                theStack[0].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[0];
                theStack[0].transform.GetChild(0).GetComponent<ResizeTexture>().meshType = 0;
            }
            else
            {
                theStack[stackIndex + 1].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[0];
                theStack[stackIndex + 1].transform.GetChild(0).GetComponent<ResizeTexture>().meshType = 0;

            }
        }
        if(combo > 0)
        {
            theStack[stackIndex].transform.GetChild(2).position = theStack[stackIndex].transform.GetChild(0).position;
            theStack[stackIndex].transform.GetChild(2).localScale = theStack[stackIndex].transform.GetChild(0).localScale;
            if(combo == 6)
            {
                theStack[stackIndex].transform.GetChild(2).GetComponent<ParticleSystem>().Play(true);
            }
            else
            {
                theStack[stackIndex].transform.GetChild(2).GetComponent<ParticleSystem>().Play(false);
            }
           
        }
        

       lastTilePosition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0)
            stackIndex = transform.childCount - 1;

        desirePosition = (Vector3.down) * scoreCount * 0.3f;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount * 0.3f, 0);
        theStack[stackIndex].transform.GetChild(0).localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

       theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().ResizeTextureUV(stackBounds.y, stackBounds.x);

        ColorMesh(theStack[stackIndex].GetComponent<MeshFilter>().mesh);
        if (stackBounds.x <= 0.3f || stackBounds.y <= 0.3f )
        {
            theStack[stackIndex].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[2];
        }

        if(scoreCount > 65)
        {
            skyParticle.transform.GetChild(0).gameObject.SetActive(false);
            skyParticle.transform.GetChild(1).gameObject.SetActive(true);
        }
       
       if(scoreCount > 15)
       {
           city.GetChild(0).gameObject.SetActive(false);
       }
    }

    private bool CheckDead()
    {
        Transform t = theStack[stackIndex].transform;

        if (isMovingOnX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            float stckTemp = stackBounds.x;
            stckTemp -= Mathf.Abs(deltaX);
            if (stckTemp <= 0)
                return false;
            else
                return true;
        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            float stckTemp = stackBounds.y;
            stckTemp -= Mathf.Abs(deltaZ);
            if (stckTemp <= 0)
                return false;
            else
                return true;
        }
    }

    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;

        if (isMovingOnX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            if(Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                //Cut The tile
                if(combo > 0)
                {
                    PanelCombo.transform.GetChild(0).GetComponent<Animator>().SetTrigger("LoseCombo");
                    comboNumberText.text = 0 + "";
                }
                combo = 0;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0)
                    return false;

                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.GetChild(0).localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                CreateRubble
                    (
                    new Vector3((t.position.x > 0) 
                    ? t.position.x + (t.GetChild(0).localScale.x / 2)
                    : t.position.x - (t.GetChild(0).localScale.x / 2), t.position.y,t.position.z),
                    new Vector3(Mathf.Abs(deltaX),1,t.GetChild(0).localScale.z)
                    );
                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount * 0.3f, lastTilePosition.z);
                theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().ResizeTextureUV(stackBounds.y, stackBounds.x);
                if (stackBounds.x <= 0.3f || stackBounds.y <= 0.3f )
                {
                    theStack[stackIndex].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[2];
                    theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().meshType = 2;
                }
                scoreCount++;
                scoreText.text = scoreCount.ToString();
                SpawnTile();
                BackgroundMove(scoreCount);
                audioSource.pitch = 1f;
                audioSource.PlayOneShot(chopClip);

            }
            else
            {
                combo++;

                if (combo >= 6)
                {
                    combo = 6;
                }

                StartCoroutine("ComboBuild");
                audioSource.pitch = (combo / 6f) + 1;
                audioSource.PlayOneShot(comboClip);
            }
        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                //Cut The tile
                if (combo > 0)
                {
                    PanelCombo.transform.GetChild(0).GetComponent<Animator>().SetTrigger("LoseCombo");
                    comboNumberText.text = 0 + "";
                }
                combo = 0;
                stackBounds.y -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0)
                    return false;

                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.GetChild(0).localScale = new Vector3(stackBounds.x,1, stackBounds.y);
                CreateRubble
                 (
                 new Vector3(t.position.x
                 , t.position.y
                 , (t.position.z > 0)
                 ? t.position.z + (t.GetChild(0).localScale.z / 2)
                 : t.position.z - (t.GetChild(0).localScale.z / 2)),
                 new Vector3(t.GetChild(0).localScale.x, 1, Mathf.Abs(deltaZ))
                 );
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount * 0.3f, middle - (lastTilePosition.z / 2));
               theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().ResizeTextureUV(stackBounds.y, stackBounds.x);
                if (stackBounds.x <= 0.3f || stackBounds.y <= 0.3f )
                {
                    theStack[stackIndex].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[2];
                    theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().meshType = 2;
                }
                scoreCount++;
                scoreText.text = scoreCount.ToString();
                SpawnTile();
                
                BackgroundMove(scoreCount);
                audioSource.pitch = 1f;
                audioSource.PlayOneShot(chopClip);
            }
            else
            {                
                combo++;

                if(combo >= 6)
                {
                    combo = 6;
                }

                StartCoroutine("ComboBuild");
                audioSource.pitch = (combo / 6f ) + 1;
                audioSource.PlayOneShot(comboClip);
            }
        }

        PanelCombo.transform.localScale = new Vector3(((combo / 6f) * 0.3f) + 0.7f, ((combo / 6f) * 0.3f) + 0.7f, ((combo / 6f) * 0.3f) + 0.7f);
        secundaryPosition = (isMovingOnX) ? t.localPosition.x : t.localPosition.z;
        isMovingOnX = !isMovingOnX;

        StartCoroutine(backgroundMusic.ComboBackground(combo));

        float s = 0;
        s = s / theStack.Length;
     
        return true;
    }

    IEnumerator ComboBuild()
    {
        PanelCombo.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Combo" + combo);
        comboNumberText.text = combo + ""; 
                
        Transform t = null;
        for (int i = 1; i <= combo; i++)
        {
            t = theStack[stackIndex].transform;
            t.localPosition = new Vector3(lastTilePosition.x, scoreCount * 0.3f, lastTilePosition.z);
            theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().ResizeTextureUV(stackBounds.y, stackBounds.x);
            if (stackBounds.x <= 0.3f || stackBounds.y <= 0.3f)
            {
                theStack[stackIndex].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = building[2];
                theStack[stackIndex].transform.GetChild(0).GetComponent<ResizeTexture>().meshType = 2;
            }
            scoreCount++;
            scoreText.text = scoreCount.ToString();
            SpawnTile();
            
            BackgroundMove(scoreCount);
            yield return new WaitForSeconds(.05f);
        }
    }

    private void ColorMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Color32[] colors = new Color32[vertices.Length];
        float f = Mathf.Sin(scoreCount * 0.25f);

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Lerp4(gameColors[0], gameColors[1], gameColors[2], gameColors[3], gameColors[4], gameColors[5], gameColors[6], gameColors[7], gameColors[8], gameColors[9], f);
            //if(i == vertices.Length - 1)
            //{
            //    Camera.main.backgroundColor = Lerp4(gameColors[0], gameColors[1], gameColors[2], gameColors[3], gameColors[4], gameColors[5], gameColors[6], gameColors[7], gameColors[8], gameColors[9], f);
            //}
        }

        mesh.colors32 = colors;

    }

    private void BackgroundMove(int value)
    {

        background.color = gradientBackground.Evaluate(value / 100f);
    }

    private Color32 Lerp4(Color32 a, Color32 b, Color32 c, Color32 d, Color32 e, Color32 f, Color32 g, Color32 h, Color32 i, Color32 j, float t)
    {
        if (t < 0.1f)
            return Color.Lerp(a, b, t / 0.1f);
        else if (t >= 0.1f && t < 0.2f)
            return Color.Lerp(b, c, (t - 0.2f) / 0.2f);
        else if (t >= 0.2f && t < 0.3f)
            return Color.Lerp(c, d, (t - 0.3f) / 0.3f);
        else if (t >= 0.3f && t < 0.4f)
            return Color.Lerp(d, e, (t - 0.4f) / 0.4f);
        else if (t >= 0.4f && t < 0.5f)
            return Color.Lerp(e, f, (t - 0.5f) / 0.5f);
        else if (t >= 0.5f && t < 0.6f)
            return Color.Lerp(f, g, (t - 0.6f) / 0.6f);
        else if (t >= 0.6f && t < 0.7f)
            return Color.Lerp(g, h, (t - 0.7f) / 0.7f);
        else if (t >= 0.7f && t < 0.8f)
            return Color.Lerp(h, i, (t - 0.8f) / 0.8f);
        else
            return Color.Lerp(i, j, (t - 0.9f) / 0.9f);
    }

    private void EndGame()
    {
        if (PlayerPrefs.GetInt("hiscore") < scoreCount)
            PlayerPrefs.SetInt("hiscore", scoreCount);

        PlayerPrefs.SetInt("score", scoreCount);
        gameOver = true;
        StartCoroutine(backgroundMusic.ComboBackground(-1));
        endPanel.SetActive(true);
        theStack[stackIndex].AddComponent<Rigidbody>();
        GameManager.Instance.EndGame();
        scoreText.gameObject.SetActive(false);
    }

    public void Die()
    {
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("hiscore") < scoreCount)
            PlayerPrefs.SetInt("hiscore", scoreCount);

        PlayerPrefs.SetInt("score", scoreCount);
        gameOver = true;
        StartCoroutine(backgroundMusic.ComboBackground(-1));
        endPanel.SetActive(true);
        theStack[stackIndex].AddComponent<Rigidbody>();
        GameManager.Instance.EndGame();
        scoreText.gameObject.SetActive(false);
    }

    public void OnButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }  
}

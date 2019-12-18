using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

public class G10_L2_Faiza : MonoBehaviour
{
   
    public InputField input;
    public Button btn;
    public Button btn2;
    public Button reset;
    public GameObject obj;
    public Text state;
    public Text steps;
    public Text message;
    public AudioSource sound;
    public AudioSource cubemusic;
    public Texture cubetexture;
    private string str = null;
    char[] word;
    private char[] copytapeChar;
    Vector3 Position;

    int counter = 0;
    int tapemove, tapemove2;
    public Vector3 header = new Vector3(-2, 2.3f, 0);
    Regex rgx = new Regex(@"[a]$");

    private Prime turing = new Prime();

    // Start is called before the first frame update
    void Start()
    {
        Position = this.transform.position;
      
        obj.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            runTM();
        }
        
    }

    private void runTM()
    {
        if (turing.current_state != States.failed && turing.current_state != States.q13)
        {
            if (turing.current_state != States.q13 || turing.moveCurrent1 != MoveTape.H && turing.moveCurrent2 != MoveTape.H)
            {
                counter = counter + 1;
                //get next state after the machine reads the current character
                turing.Read();
                word[turing.lastPosition1] = turing.replaceChar1;
                copytapeChar[turing.lastPosition2] = turing.replaceChar2;
                turing.currentChar1 = word[turing.position1];
                turing.currentChar2 = copytapeChar[turing.position2];
                changeTapeCharacter();
                cubemusic.Play();
                if (turing.moveCurrent1 == MoveTape.R)
                    tapemove = -1;
                else if (turing.moveCurrent1 == MoveTape.L)
                    tapemove = 1;
                else
                    tapemove = 0;

                if (turing.moveCurrent2 == MoveTape.R)
                    tapemove2 = -1;
                else if (turing.moveCurrent2 == MoveTape.L)
                    tapemove2 = 1;
                else
                    tapemove2 = 0;
                for (int i = 0; i < copytapeChar.Length; i++)
                {
                    GameObject tape1move = GameObject.Find("cube" + i);
                    tape1move.transform.position = new Vector3(tape1move.transform.position.x + (tapemove * 150f), tape1move.transform.position.y, tape1move.transform.position.z);
                    GameObject tape2move = GameObject.Find("tape1" + i);
                    tape2move.transform.position = new Vector3(tape2move.transform.position.x + (tapemove2 * 150f), tape2move.transform.position.y, tape2move.transform.position.z);
                }


                //  this.transform.position = new Vector3(Position.x - 3 + (turing.position1 - 1) * 3.0f, Position.y, Position.z);
               

                header = this.transform.position;
            }
            state.text = "current State: Q0" + turing.current_state;
            steps.text = "Step: " + copytapeChar[turing.position2].ToString();
            message.text = word[turing.position1].ToString();
        }

        if (turing.current_state == States.q13 && turing.moveCurrent1 == MoveTape.H && turing.moveCurrent2 == MoveTape.H)
        {
            // turing.current_state = States.failed;
            message.color = Color.green;
            message.text = "accepted";
            reset.gameObject.SetActive(true);
            btn2.gameObject.SetActive(true);
        }
        else if (turing.current_state != States.q13 && turing.moveCurrent1 == MoveTape.H && turing.moveCurrent2 == MoveTape.H)
        {
            turing.current_state = States.failed;
            message.color = Color.red;
            message.text = "rejected";
            reset.gameObject.SetActive(true);
        }
    }

    private void tapeDisplay()
    {
        float cubepos = -300;

        obj.gameObject.SetActive(true);

        for (int i = 0; i < word.Length; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(420f + cubepos, 180f, 320f);
            cube.transform.localScale = new Vector3(40f, 40f, 40f);
            cube.name = "cube" + i.ToString();
            var rend = cube.GetComponent<Renderer>();
            rend.material.SetTexture("_MainTex", cubetexture);
            GameObject childObject = new GameObject("print");
            childObject.transform.parent = cube.transform;
            childObject.AddComponent<TextMesh>();
            childObject.GetComponent<TextMesh>().fontSize = 300;
            childObject.GetComponent<TextMesh>().characterSize = 0.1f;
            childObject.GetComponent<TextMesh>().color = Color.red;
            childObject.GetComponent<TextMesh>().transform.localScale = new Vector3(0.36f, 0.36f, 0.36f);
            childObject.GetComponent<TextMesh>().transform.localPosition = new Vector3(-0.3f, 0.7f, -0.3f);
          
            childObject.GetComponent<TextMesh>().text = word[i].ToString();
            cubepos = cubepos + 150.0f;
        }
     
    }
    private void tape2Display()
    {
        float cubepos = -300;

        obj.gameObject.SetActive(true);

        for (int i = 0; i < copytapeChar.Length; i++)
        {
            GameObject tape = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tape.transform.position = new Vector3(420f + cubepos, 130f, 320f);
            tape.transform.localScale = new Vector3(40f, 40f, 40f);
            tape.name = "tape1" + i.ToString();

            GameObject childObject = new GameObject("print11");
            childObject.transform.parent = tape.transform;
            childObject.AddComponent<TextMesh>();
            childObject.GetComponent<TextMesh>().fontSize = 300;
            childObject.GetComponent<TextMesh>().characterSize = 0.1f;
            childObject.GetComponent<TextMesh>().color = Color.red;
            childObject.GetComponent<TextMesh>().transform.localScale = new Vector3(0.36f, 0.36f, 0.36f);
            childObject.GetComponent<TextMesh>().transform.localPosition = new Vector3(-0.3f, 0.7f, -0.3f);
            
            childObject.GetComponent<TextMesh>().text = copytapeChar[i].ToString();
            cubepos = cubepos + 150.0f;
        }
    }
    private void changeTapeCharacter()
    {
        GameObject tape = GameObject.Find("cube" + turing.lastPosition1.ToString());
        tape.GetComponentInChildren<TextMesh>().text = turing.replaceChar1.ToString();
        GameObject tape1 = GameObject.Find("tape1" + turing.lastPosition2.ToString());
        tape1.GetComponentInChildren<TextMesh>().text = turing.replaceChar2.ToString();
     
    }


    public void inputget()
    {
        str = "ΔΔ" + str + "ΔΔ";
        word = str.ToCharArray();
        copytapeChar = str.ToCharArray();
        for (int i = 0; i < word.Length; i++)
        {
            copytapeChar[i] = 'Δ';
        }
        tapeDisplay();
        tape2Display();
        turing.currentChar1 = word[turing.position1];
        turing.currentChar2 = copytapeChar[turing.position2];
        input.gameObject.SetActive(false);
        btn.gameObject.SetActive(false);
        btn2.gameObject.SetActive(false);
        state.text = "Current State: q0";
        steps.text = "step: 0";
    }

    public void resetTM()
    {
        for (int i = 0; i < word.Length; i++)
        {
            GameObject go = GameObject.Find("cube" + i.ToString());
            GameObject go1 = GameObject.Find("tape1" + i.ToString());
            //if the object exist then destroy it
            if (go && go1)
            {
                Destroy(go.gameObject);
                Destroy(go1.gameObject);
            }
        }
        str = "";
        input.text = "";
        input.gameObject.SetActive(true);
        btn.gameObject.SetActive(true);
        reset.gameObject.SetActive(false);
        message.text = "";
        steps.text = "step: 0";
        state.text = "Current State: q0";
    }

    public void validate()
    {
        if (input.text == null)
        {
            str = "";
        }
        else if (rgx.IsMatch(input.text))
        {
            str = input.text;
        }
        else
        {
            input.text = str;
        }
    }
    public void changeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}

class Prime
{
    public States current_state = States.q0;
    public int position1 = 2;
    public int position2 = 2;
    public int lastPosition1;
    public int lastPosition2;

    public char currentChar1;
    public char currentChar2;
    public char symbol = 'Δ';
    public char replaceChar1;
    public char replaceChar2;
    public MoveTape moveCurrent1;
    public MoveTape moveCurrent2;

    public void Read()
    {
        switch (current_state)
        {
            case States.q0:
                {
                    if (currentChar1 == 'a' && currentChar2 == symbol)
                    {

                        current_state = States.q0;
                        moveCurrent1 = MoveTape.R;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = 'a';
                        replaceChar2 = 'x';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 + 1;
                        position2 = position2 + 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q1;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                        position2 = position2 - 1;
                    }
                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                    }
                    break;
                }
            case States.q1:
                {
                    if (currentChar1 == 'a' && currentChar2 == 'x')
                    {
                        current_state = States.q1;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = 'a';
                        replaceChar2 = 'x';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                        position2 = position2 - 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q2;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 + 1;
                    }
                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }
            case States.q2:
                {
                    if (currentChar1 == symbol && currentChar2 == 'x')
                    {
                        current_state = States.q3;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 + 1;
                    }

                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }
            case States.q3:
                {
                    if (currentChar1 == symbol && currentChar2 == 'x')
                    {
                        current_state = States.q4;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 + 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q14;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                    }
                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                    }
                    break;
                }
            case States.q4:
                {
                    if (currentChar1 == symbol && currentChar2 == 'x')
                    {
                        current_state = States.q5;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 + 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q13;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                    }
                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                    }
                    break;
                }
            case States.q5:
                {
                    if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q6;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == 'x')
                    {
                        current_state = States.q6;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = symbol;
                        replaceChar2 = 'x';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }

                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }
            case States.q6:
                {
                    if (currentChar1 == symbol && currentChar2 == 'y')
                    {
                        current_state = States.q6;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q7;
                        moveCurrent1 = MoveTape.R;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 + 1;
                        position2 = position2 + 1;
                    }

                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }
            case States.q7:
                {
                    if (currentChar1 == 'a' && currentChar2 == 'y')
                    {
                        current_state = States.q7;
                        moveCurrent1 = MoveTape.R;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = 'a';
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 + 1;
                        position2 = position2 + 1;
                    }
                    else if (currentChar1 == 'B' && currentChar2 == 'y')
                    {
                        current_state = States.q7;
                        moveCurrent1 = MoveTape.R;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = 'B';
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 + 1;
                        position2 = position2 + 1;
                    }
                    else if (currentChar1 == 'B' && currentChar2 == 'x')
                    {
                        current_state = States.q8;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = 'B';
                        replaceChar2 = 'x';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                        position2 = position2 - 1;
                    }
                    else if (currentChar1 == 'a' && currentChar2 == 'x')
                    {
                        current_state = States.q8;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = 'a';
                        replaceChar2 = 'x';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                        position2 = position2 - 1;
                    }
                    else if (currentChar1 == 'a' && currentChar2 == symbol)
                    {
                        current_state = States.q8;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = 'a';
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                        position2 = position2 - 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == 'y')
                    {
                        current_state = States.q10;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                    }

                    else if (currentChar1 == symbol && currentChar2 == 'x')
                    {
                        current_state = States.q14;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = symbol;
                        replaceChar2 = 'x';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                    }
                    else if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q14;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                    }

                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }
            case States.q8:
                {
                    if (currentChar1 == 'a' && currentChar2 == 'y')
                    {
                        current_state = States.q9;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = 'B';
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }
                    else if (currentChar1 == 'B' && currentChar2 == 'y')
                    {
                        current_state = States.q9;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = 'B';
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }

                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }

            case States.q9:
                {
                    if (currentChar1 == 'B' && currentChar2 == symbol)
                    {
                        current_state = States.q7;
                        moveCurrent1 = MoveTape.R;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = 'B';
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 + 1;
                        position2 = position2 + 1;
                    }
                    else if (currentChar1 == 'B' && currentChar2 == 'y')
                    {
                        current_state = States.q9;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = 'B';
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }

                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }

            case States.q10:
                {
                    if (currentChar1 == 'a' && currentChar2 == 'y')
                    {
                        current_state = States.q10;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = 'a';
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                    }
                    else if (currentChar1 == 'B' && currentChar2 == 'y')
                    {
                        current_state = States.q10;
                        moveCurrent1 = MoveTape.L;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = 'B';
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position1 = position1 - 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == 'y')
                    {
                        current_state = States.q11;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 + 1;
                    }

                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }
            case States.q11:
                {
                    if (currentChar1 == symbol && currentChar2 == 'y')
                    {
                        current_state = States.q11;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.R;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 + 1;
                    }
                    else if (currentChar1 == symbol && currentChar2 == symbol)
                    {
                        current_state = States.q13;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                        replaceChar1 = symbol;
                        replaceChar2 = symbol;
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                    }
                    else if (currentChar1 == symbol && currentChar2 == 'x')
                    {
                        current_state = States.q12;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }
                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;
                    }
                    break;
                }
            case States.q12:
                {
                    if (currentChar1 == symbol && currentChar2 == 'y')
                    {
                        current_state = States.q6;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.L;
                        replaceChar1 = symbol;
                        replaceChar2 = 'y';
                        lastPosition1 = position1;
                        lastPosition2 = position2;
                        position2 = position2 - 1;
                    }
                    else
                    {
                        current_state = States.failed;
                        moveCurrent1 = MoveTape.H;
                        moveCurrent2 = MoveTape.H;

                    }
                    break;
                }

        }

    }

}
public enum States
{
    q0, q1, q2, q3, q4, q5, q6, q7, q8, q9, q10, q11, q12, q13, q14, failed,
}
public enum MoveTape
{
    L,
    H,
    R
}

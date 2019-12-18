using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class G10_L3_Abdul : MonoBehaviour
{
   
    public AudioSource sound;
    public AudioSource music;
    public Texture cubetexture;
    private string str = null;
    static string symbol = "ΔΔ";
    char[] word;
    Vector3 Position;
    int move = 1;
    int counter = 0;
    char[] split;
    public Vector3 direction;
    Regex expr = new Regex(@"[a-b]$");
    public InputField input;
    public Button btn;
    public Button menu;
    public Button reset;
    public GameObject obj;
    public Text state;
    public Text TransitionsCount;
    public Text outputText;
    private abdulTM tm = new abdulTM();

    // Start is called before the first frame update
    void Start()
    {
        Position = this.transform.position;
        direction = Position;
        obj.transform.position = new Vector3(490f, 100, 423);
        obj.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Calling the turing Machine for Transition Move
            runTM();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (move > 0)
            {
                move = move - 1;
                direction.z = direction.z - 120f;
                this.transform.position = new Vector3(direction.x, direction.y, direction.z);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (move < word.Length - 1)
            {
                move = move + 1;
                direction.z = direction.z + 120f;
                this.transform.position = new Vector3(direction.x, direction.y, direction.z);
            }
        }
    }

    private void runTM()
    {
        if (tm.currentState != null)
        {
            if (tm.currentState != "q14" || tm.lastmove != TransitionMove.H)
            {
                counter = counter + 1;
                //get next state after the machine reads the current character
                tm.exeTM();
                word[tm.poscube] = tm.writeCharacter;
                tm.character = word[tm.pos];
                changeCharacter();
                music.Play();
                for (int i = 0; i < split.Length; i++)
                {
                    GameObject tape1move = GameObject.Find("cell" + i);
                    tape1move.transform.position = new Vector3(tape1move.transform.position.x, tape1move.transform.position.y, tape1move.transform.position.z + (tm.tapemove * -120f));
                }
                
                direction = this.transform.position;
            }
        }
        state.text = "current State: " + tm.currentState;
        TransitionsCount.text = "Step: " + counter.ToString();

        if (tm.currentState == "q10" && tm.lastmove == TransitionMove.H)
        {

            outputText.color = Color.green;
            outputText.text = "string is accepted";
            state.text = "current State: q10";
            reset.gameObject.SetActive(true);
            menu.gameObject.SetActive(true);
        }
        else if (tm.currentState != "q10" && tm.lastmove == TransitionMove.H)
        {
            outputText.color = Color.red;
            outputText.text = "string is rejected";
            reset.gameObject.SetActive(true);
            menu.gameObject.SetActive(true);
        }
    }
    // generate the tape which contains the strings and is read and write by the turing machine
    private void tapeGenerate()
    {
        float cellpos = 240;
        split = word.ToArray<char>();
        obj.gameObject.SetActive(true);

        //for loop is used generate numbber of cells for each alhpabet of string 
        for (int i = 0; i < split.Length; i++)
        {
            GameObject tape = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tape.transform.position = new Vector3(490f, 140, Position.z + cellpos);
         
            tape.transform.localScale = new Vector3(40f, 40f, 40f);
            tape.name = "cell" + i.ToString();
            var rend = tape.GetComponent<Renderer>();
            rend.material.SetTexture("_MainTex", cubetexture);
            GameObject childObject = new GameObject("print");
            childObject.transform.parent = tape.transform;
            childObject.AddComponent<TextMesh>();
            // Text mesh is used to display alpha bet on the cell
            childObject.GetComponent<TextMesh>().fontSize = 300;
            childObject.GetComponent<TextMesh>().characterSize = 0.1f;
            childObject.GetComponent<TextMesh>().color = Color.red;
            childObject.GetComponent<TextMesh>().transform.localScale = new Vector3(0.37147f, 0.37147f, 0.37147f);
            childObject.GetComponent<TextMesh>().transform.localPosition = new Vector3(-0.5f, 0.65f, 0.3f);
            childObject.GetComponent<TextMesh>().transform.Rotate(0f, 90f, 0f);
            childObject.GetComponent<TextMesh>().text = split[i].ToString();
            cellpos = cellpos - 120.0f;
        }
    }

    // This function is called when a character alphabet is required to change on tape
    private void changeCharacter()
    {
        GameObject find = GameObject.Find("cell" + tm.poscube.ToString());
        find.GetComponentInChildren<TextMesh>().text = tm.writeCharacter.ToString();
    }

    // get the input string from the user at run time
    public void getString()
    {
        if (input.text!=null)
        {
            str = symbol + str + symbol;
            word = str.ToCharArray();
            tapeGenerate();
            tm.character = word[tm.poscube];
            input.gameObject.SetActive(false);
            btn.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);
            reset.gameObject.SetActive(false);
            state.text = "Current State: q0";
            TransitionsCount.text = "step: 0";
        }
       
    }


    //reset the turing machine to its default and ready for next input
    public void restoreTuringMachine()
    {
        for (int i = 0; i < word.Length; i++)
        {
            GameObject go = GameObject.Find("cell" + i.ToString());
            //if the object exist then destroy it
            if (go)
                Destroy(go.gameObject);
        }
        str = "";
        input.gameObject.SetActive(true);
        btn.gameObject.SetActive(true);
        reset.gameObject.SetActive(false);
        outputText.text = "";
        TransitionsCount.text = "step: 0";
        input.text = "";
    }

    // check string validation on input
    public void checkValidation()
    {
        if (input.text == null)
        {
            str = "";
        }
        else if (expr.IsMatch(input.text))
        {
            str = input.text;
        }
        else
        {
            input.text = str;
        }
    }
    public void nextScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}

// here is turing machine class which contains hardcoded turing machine by using list data structure
public class abdulTM
{
    public string currentState = null;
    private string nextSt;
    public char character;
    public char writeCharacter;
    public int tapemove = 0;
    public int pos = 2;
    public int poscube = 2;
    public TransitionMove lastmove;
    public List<StateTr> state0 = new List<StateTr>() {new StateTr(){ readChar = 'a', nxtstate = "q1", writeChar = 'x', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'x',nxtstate = "q0",writeChar = 'x', move = TransitionMove.R},
                                                new StateTr(){ readChar = 'y',nxtstate = "q6",writeChar = 'y', move = TransitionMove.R}};

    public List<StateTr> state1 = new List<StateTr>() {new StateTr(){ readChar = 'a', nxtstate = "q1", writeChar = 'a', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'y',nxtstate = "q1",writeChar = 'y', move = TransitionMove.R},
                                                  new StateTr(){ readChar = 'x',nxtstate = "q2",writeChar = 'x', move = TransitionMove.R},
                                                new StateTr(){ readChar = 'b',nxtstate = "q2",writeChar = 'y', move = TransitionMove.R}};

    public List<StateTr> state2 = new List<StateTr>() {new StateTr(){ readChar = 'x', nxtstate = "q2", writeChar = 'x', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'b',nxtstate = "q2",writeChar = 'b', move = TransitionMove.R},
                                                new StateTr(){ readChar = 'a',nxtstate = "q3",writeChar = 'x', move = TransitionMove.R}};

    public List<StateTr> state3 = new List<StateTr>() {new StateTr(){ readChar = 'a', nxtstate = "q3", writeChar = 'a', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'y',nxtstate = "q4",writeChar = 'y', move = TransitionMove.R},
                                                new StateTr(){ readChar = 'b',nxtstate = "q5",writeChar = 'y', move = TransitionMove.L}};

    public List<StateTr> state4 = new List<StateTr>() {new StateTr(){ readChar = 'y', nxtstate = "q4", writeChar = 'y', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'b',nxtstate = "q5",writeChar = 'y', move = TransitionMove.L},
                                                new StateTr(){ readChar = 'Δ',nxtstate = "q5",writeChar = 'Δ', move = TransitionMove.L}};

    public List<StateTr> state5 = new List<StateTr>() {new StateTr(){ readChar = 'a', nxtstate = "q5", writeChar = 'a', move = TransitionMove.L } ,
                                                new StateTr(){ readChar = 'b',nxtstate = "q5",writeChar = 'b', move = TransitionMove.L},
                                                new StateTr(){ readChar = 'x',nxtstate = "q5",writeChar = 'x', move = TransitionMove.L},
                                                new StateTr(){ readChar = 'y',nxtstate = "q5",writeChar = 'y', move = TransitionMove.L},                                               
                                                new StateTr(){ readChar = 'b',nxtstate = "q8",writeChar = 'y', move = TransitionMove.L},
                                                new StateTr(){ readChar = 'Δ',nxtstate = "q0",writeChar = 'Δ', move = TransitionMove.R}};

    public List<StateTr> state6 = new List<StateTr>() {new StateTr(){ readChar = 'y', nxtstate = "q6", writeChar = 'y', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'b',nxtstate = "q7",writeChar = 'y', move = TransitionMove.R},
                                                new StateTr(){ readChar = 'x',nxtstate = "q9",writeChar = 'x', move = TransitionMove.R}};
    public List<StateTr> state7 = new List<StateTr>() {new StateTr(){ readChar = 'b', nxtstate = "q7", writeChar = 'b', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'x',nxtstate = "q8",writeChar = 'x', move = TransitionMove.R},};

    public List<StateTr> state8 = new List<StateTr>() {new StateTr(){ readChar = 'x', nxtstate = "q8", writeChar = 'x', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'y',nxtstate = "q8",writeChar = 'y', move = TransitionMove.R},
                                                };
    public List<StateTr> state9 = new List<StateTr>() {new StateTr(){ readChar = 'x', nxtstate = "q9", writeChar = 'x', move = TransitionMove.R } ,
                                                new StateTr(){ readChar = 'y',nxtstate = "q9",writeChar = 'y', move = TransitionMove.R},
                                                new StateTr(){ readChar = 'Δ',nxtstate = "q10",writeChar = 'Δ', move = TransitionMove.H}};

    public List<StateTr> state10 = new List<StateTr>() { new StateTr() { readChar = 'Δ', nxtstate = "q10", writeChar = 'Δ', move = TransitionMove.H } };

    List<Table> transitionTable;
    public abdulTM()
    {
        transitionTable = new List<Table>(){new Table(){states="q0",trans=state0},
                                                        new Table(){states="q1",trans=state1},
                                                        new Table(){states="q2",trans=state2},
                                                        new Table(){states="q3",trans=state3},
                                                        new Table(){states="q4",trans=state4},
                                                        new Table(){states="q5",trans=state5},
                                                        new Table(){states="q6",trans=state6},
                                                        new Table(){states="q7",trans=state7},
                                                        new Table(){states="q8",trans=state8},
                                                        new Table(){states="q9",trans=state9},
                                                        new Table(){states="q10",trans=state10},
        };
        currentState = "q0";
    }

    //This function is used to execute the turing machine for each transition
    public void exeTM()
    {
        Table stateTransitions = transitionTable.Find(x => x.states == currentState);
        StateTr currentTransition = stateTransitions.trans.Find(x => x.readChar == character);
        currentState = currentTransition.nxtstate;
        writeCharacter = currentTransition.writeChar;
        lastmove = currentTransition.move;
        poscube = pos;
        if (lastmove == TransitionMove.L)
        {
            pos = pos - 1;
            tapemove = 1;
        }
        else if (lastmove == TransitionMove.R)
        {
            pos = pos + 1;
            tapemove = -1;
        }
        else
            tapemove = 0;
    }
}
public struct StateTr
{
    public char readChar { get; set; }
    public string nxtstate { get; set; }
    public char writeChar { get; set; }
    public TransitionMove move { get; set; }
}
public class Table
{
    public string states { get; set; }
    public List<StateTr> trans = new List<StateTr>();
}
public enum TransitionMove { 
    L,R,H
}

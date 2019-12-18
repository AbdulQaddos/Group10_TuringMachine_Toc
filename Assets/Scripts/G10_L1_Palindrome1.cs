using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class G10_L1_Palindrome1 : MonoBehaviour
{
    public InputField input;
    public Button btn;
    public Button mainmenu;
    public Button reset;
    public GameObject obj;
    public Text state;
    public Text steps;
    public Text message;
    public AudioSource sound;
    private string str = null;
    static char symbol = 'Δ';
    public Texture cubetexture;
    List<char> word;
    Vector3 Position;
    int move = 1;
    int tapemove;
    int counter = 1;
    char[] split;
    public Vector3 camerMove = new Vector3(-2, 2.3f, 0);
    Regex rgx = new Regex(@"[2-9]$");

    private TuringMachine tm;
    private Transition trans;

    // Start is called before the first frame update
    void Start()
    {
        Position = this.transform.position;
        obj = GameObject.Find("Capsule");
        obj.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            runTM();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (move > 0)
            {
                move = move - 1;
                camerMove.x = camerMove.x - 2;
                this.transform.position = new Vector3(camerMove.x, 2.3f, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (move < word.Count - 1)
            {
                move = move + 1;
                camerMove.x = camerMove.x + 2;
                this.transform.position = new Vector3(camerMove.x, 2.3f, 0);
            }
        }
    }

    private void runTM()
    {
        if (tm.currentState != null)
        {
            if (!tm.currentState.isEnd() || tm.lastMovement != Movement.H)
            {
                counter = counter + 1;
                state.text = "current State: " + tm.currentState.stateNam();
                steps.text = "Step: " + counter.ToString();
                trans = tm.currentState.apply(tm.getCurrentChar()); //get next state after the machine reads the current character
                if (trans != null)
                {
                    tm.modifyCharAtPosition(trans.getExchangeChar());
                    changeCharacter();
                    sound.Play();
                    if (tm.lastMovement == Movement.L)
                        tapemove = 1;

                    else if (tm.lastMovement == Movement.R)
                        tapemove = -1;
                    else
                        tapemove = 0;
                    GameObject tape0move = GameObject.Find("cube");
                    tape0move.transform.position = new Vector3(tape0move.transform.position.x, tape0move.transform.position.y, tape0move.transform.position.z + (tapemove * 200f));

                    for (int i = 0; i < split.Length; i++)
                    {
                        GameObject tape1move = GameObject.Find("cube" + i);
                        tape1move.transform.position = new Vector3(tape1move.transform.position.x, tape1move.transform.position.y, tape1move.transform.position.z + (tapemove * 200f));
                    }
                    GameObject tape2move = GameObject.Find("cuben");
                    tape2move.transform.position = new Vector3(tape2move.transform.position.x, tape2move.transform.position.y, tape2move.transform.position.z  + (tapemove * 200f));

                    // this.transform.position = new Vector3(Position.x + (tm.position - 1) * 2.0f, Position.y, Position.z);
                   
                    camerMove = this.transform.position;

                    tm.modifyPosition(trans.getMovement());

                    tm.currentState = trans.getNextState();
                    tm.lastMovement = trans.getMovement();
                }
                else
                {
                    tm.currentState = null;
                }
            }
        }
        if (tm.currentState.isEnd() && tm.lastMovement == Movement.H)
        {
            message.color = Color.green;
            message.text = "string is accepted";
            reset.gameObject.SetActive(true);
            mainmenu.gameObject.SetActive(true);
        }
        else if (!tm.currentState.isEnd() && tm.lastMovement == Movement.H)
        {
            message.color = Color.red;
            message.text = "string is rejected";
            reset.gameObject.SetActive(true);
            mainmenu.gameObject.SetActive(true);
        }
    }

    private void diplayInput()
    {
        float cubepos = -200;
        split = word.ToArray<char>();
        obj.gameObject.SetActive(true);

        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.transform.position = new Vector3(386f, 110, Position.z + cubepos);
        cube1.transform.localScale = new Vector3(40f, 40f, 40f);
        cube1.name = "cube";
        var rend = cube1.GetComponent<Renderer>();
        rend.material.SetTexture("_MainTex", cubetexture);
        GameObject childObject1 = new GameObject("print");
        childObject1.transform.parent = cube1.transform;
        childObject1.AddComponent<TextMesh>();
        childObject1.GetComponent<TextMesh>().fontSize = 300;
        childObject1.GetComponent<TextMesh>().characterSize = 0.1f;
        childObject1.GetComponent<TextMesh>().color = Color.red;
        childObject1.GetComponent<TextMesh>().transform.localScale = new Vector3(0.37147f, 0.37147f, 0.37147f);
        childObject1.GetComponent<TextMesh>().transform.localPosition = new Vector3(0.5491577f, 0.7f, -0.3f);
        childObject1.GetComponent<TextMesh>().transform.Rotate(0.531f, -88.56901f, 1.161f);
        childObject1.GetComponent<TextMesh>().text = symbol.ToString();
        cubepos = cubepos + 200.0f;
        for (int i = 0; i < split.Length; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(386f, 110, Position.z + cubepos);
            cube.name = "cube" + i.ToString();
            cube.transform.localScale = new Vector3(40f, 40f, 40f);
            var rend1 = cube.GetComponent<Renderer>();
            rend1.material.SetTexture("_MainTex", cubetexture);
            GameObject childObject = new GameObject("print");
            childObject.transform.parent = cube.transform;
            childObject.AddComponent<TextMesh>();
            childObject.GetComponent<TextMesh>().fontSize = 300;
            childObject.GetComponent<TextMesh>().characterSize = 0.1f;
            childObject.GetComponent<TextMesh>().color = Color.red;
            childObject.GetComponent<TextMesh>().transform.localScale = new Vector3(0.37147f, 0.37147f, 0.37147f);
            childObject.GetComponent<TextMesh>().transform.localPosition = new Vector3(0.5491577f, 0.7f, -0.3f);
            childObject.GetComponent<TextMesh>().transform.Rotate(0.531f, -88.56901f, 1.161f);
            childObject.GetComponent<TextMesh>().text = split[i].ToString();
            cubepos = cubepos + 200.0f;
        }
        this.transform.position = new Vector3(Position.x, Position.y, Position.z);
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = new Vector3(386f, 110, Position.z + cubepos);
        cube2.transform.localScale = new Vector3(40f, 40f, 40f);
        cube2.name = "cuben";
        var rend2 = cube2.GetComponent<Renderer>();
        rend2.material.SetTexture("_MainTex", cubetexture);
        GameObject childObject2 = new GameObject("print");
        childObject2.transform.parent = cube2.transform;
        childObject2.AddComponent<TextMesh>();
        childObject2.GetComponent<TextMesh>().fontSize = 300;
        childObject2.GetComponent<TextMesh>().characterSize = 0.1f;
        childObject2.GetComponent<TextMesh>().color = Color.red;
        childObject2.GetComponent<TextMesh>().transform.localScale = new Vector3(0.37147f, 0.37147f, 0.37147f);
        childObject2.GetComponent<TextMesh>().transform.localPosition = new Vector3(0.5491577f, 0.7f, -0.3f);
        childObject2.GetComponent<TextMesh>().transform.Rotate(0.531f, -88.56901f, 1.161f);
        childObject2.GetComponent<TextMesh>().text = symbol.ToString();
    }

    private void changeCharacter()
    {
        GameObject find = GameObject.Find("cube" + tm.position.ToString());
        find.GetComponentInChildren<TextMesh>().text = trans.getExchangeChar().ToString();
    }


    public void inputget()
    {
        str = symbol + str + symbol;
        word = str.ToList();
        diplayInput();
        TransitionTable TT = new TransitionTable();
        tm = new TuringMachine(TT.transition_table(), word, symbol);
        input.gameObject.SetActive(false);
        btn.gameObject.SetActive(false);
        mainmenu.gameObject.SetActive(true);
        state.text = "Current State: q0";
        steps.text = "step: 0";
    }

    public void resetTM()
    {
        GameObject go1 = GameObject.Find("cube");
        if (go1)
            Destroy(go1.gameObject);
        for (int i = 0; i < word.Count; i++)
        {
            GameObject go = GameObject.Find("cube" + i.ToString());
            //if the object exist then destroy it
            if (go)
                Destroy(go.gameObject);
        }
        GameObject go2 = GameObject.Find("cuben");
        if (go2)
            Destroy(go2.gameObject);
        input.gameObject.SetActive(true);
        btn.gameObject.SetActive(true);
        reset.gameObject.SetActive(false);
        message.text = "";
        steps.text = "step: 0";
        input.text = "";
    }

    public void validate()
    {
        if (input.text == null)
        {
            str = "";
        }
        else if (!rgx.IsMatch(input.text))
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

class State
{
    private Dictionary<char, Transition> dict = new Dictionary<char, Transition>();
    private bool isEndState;
    private string stateName;
    public State(char[] letters, Transition[] transitions, bool isEndState, string name)
    {
        for (int i = 0; i < letters.Length; i++)
        {
            dict.Add(letters[i], transitions[i]);
        }
        this.isEndState = isEndState;
        this.stateName = name;
    }

    public Transition apply(char currentChar)
    {
        Transition transition = null;
        dict.TryGetValue(currentChar, out transition);
        return transition;
    }
    public bool isEnd()
    {
        return isEndState;
    }
    public string stateNam()
    {
        return stateName;
    }
}
class Transition
{
    private Tuple<Lazy<State>, char, Movement> transition;

    public Transition(Lazy<State> state, char exchangeChar, Movement move)
    {
        transition = new Tuple<Lazy<State>, char, Movement>(state, exchangeChar, move);
    }

    public State getNextState()
    {
        return transition.Item1.Value;
    }
    public char getExchangeChar()
    {
        return transition.Item2;
    }

    public Movement getMovement()
    {
        return transition.Item3;
    }
}

class TransitionTable
{
    static char symbol = 'Δ';

    public List<State> transition_table()
    {
        State state1 = null;
        State state2 = null;
        State state3 = null;
        State state4 = null;
        State state5 = null;
        State state6 = null;
        State state7 = null;



        state1 = new State(new char[] { '1', '0', symbol }, new Transition[] {new Transition(new Lazy<State>(() => state3), symbol, Movement.R),
						new Transition(new Lazy<State>(() => state2), symbol, Movement.R),
						new Transition(new Lazy<State>(() => state7), symbol, Movement.H)}, false, "q0");

        state2 = new State(new char[] { '1', '0', symbol }, new Transition[] {new Transition(new Lazy<State>(() => state2), '1', Movement.R),
						new Transition(new Lazy<State>(() => state2), '0', Movement.R),
						new Transition(new Lazy<State>(() => state6), symbol, Movement.L)}, false, "q1");

        state6 = new State(new char[] { '1', '0', symbol }, new Transition[] {new Transition(new Lazy<State>(() => state6), '1', Movement.H),
						new Transition(new Lazy<State>(() => state5), symbol, Movement.L),
						new Transition(new Lazy<State>(() => state7), symbol, Movement.H)}, false, "q2");

        state3 = new State(new char[] { '1', '0', symbol }, new Transition[] {new Transition(new Lazy<State>(() => state3), '1', Movement.R),
						new Transition(new Lazy<State>(() => state3), '0', Movement.R),
						new Transition(new Lazy<State>(() => state4), symbol, Movement.L)}, false, "q3");

        state4 = new State(new char[] { '1', '0', symbol }, new Transition[] {new Transition(new Lazy<State>(() => state5), symbol, Movement.L),
						new Transition(new Lazy<State>(() => state4), '0', Movement.H),
						new Transition(new Lazy<State>(() => state7), symbol, Movement.H)}, false, "q4");

        state5 = new State(new char[] { '1', '0', symbol },
                    new Transition[] {new Transition(new Lazy<State>(() => state5), '1', Movement.L),
						new Transition(new Lazy<State>(() => state5), '0', Movement.L),
						new Transition(new Lazy<State>(() => state1), symbol, Movement.R)}, false, "q5");
        state7 = new State(new char[] { symbol },
                    new Transition[] { new Transition(new Lazy<State>(() => state7), symbol, Movement.H) }, true, "q6");
        return new List<State>() { state1, state2, state3, state4, state5, state6, state7 };
    }
}

class TuringMachine
{
    private List<State> states = new List<State>();
    public State currentState;
    public List<char> word = new List<char>();
    public int position = 1;
    public Movement lastMovement = Movement.R;
    private char symbol;

    public TuringMachine(List<State> states, List<char> word, char symbol)
    {
        this.states = states;
        this.word = word;
        currentState = states[0];
        this.symbol = symbol;
    }

    public char getCurrentChar()
    {
        if (position < 0)
        {
            word.Insert(0, symbol); //insert new symbol if machine reads behind first word symbol
            position = 0;
        }
        else if (position >= word.Count)
            word.Add(symbol);
        return word[position];
    }

    public void modifyCharAtPosition(char c)
    {
        word[position] = c;
    }

    public void modifyPosition(Movement movement)
    {
        position += (int)movement;
    }
}
public enum Movement
{
    L = -1,
    H = 0,
    R = 1
}


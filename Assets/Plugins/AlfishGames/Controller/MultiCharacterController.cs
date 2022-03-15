using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AlfishGames/Controller/Multi Character Controller")]
public class MultiCharacterController : MonoBehaviour
{
    public Character characterType;
    private GameObject[] characters;
    private GameObject character;
    private Animator[] animators;
    private Animator anim;
    private int characterCount;

    private void Start()
    {
        GetVariablesOnStart();
        GetCharacterComponents();
        OpenCharacter();
    }

    private void GetVariablesOnStart()
    {
        characterCount = transform.childCount;
        characters = new GameObject[characterCount];
        animators = new Animator[characterCount];    
    }

    private void GetCharacterComponents()
    {
        for (int i = 0; i < characterCount; i++)
        {
            characters[i] = transform.GetChild(i).gameObject;
            animators[i] = characters[i].GetComponent<Animator>();
        }
    }

    private void CloseCharacters()
    {
        for (int i = 0; i < characterCount; i++)
        {
            characters[i].SetActive(false);
        }
    }

    private void OpenCharacter()
    {
        CloseCharacters();

        int index = (int)characterType;

        if (index < characterCount)
        {
            character = characters[index];
            anim = animators[index];
        }

        else
        {
            int randomIndex = Random.Range(0, characterCount);

            character = characters[randomIndex];
            anim = animators[randomIndex];
        }
    }
    public void OpenCharacter(Character type)
    {
        CloseCharacters();

        int index = (int)type;

        if (index < characterCount)
        {
            character = characters[index];
            anim = animators[index];
        }

        else
        {
            int randomIndex = Random.Range(0, characterCount);

            character = characters[randomIndex];
            anim = animators[randomIndex];
        }
    }  
    public void OpenCharacter(int index)
    {
        CloseCharacters();

        if (index < characterCount)
        {
            character = characters[index];
            anim = animators[index];
        }

        else
        {
            int randomIndex = Random.Range(0, characterCount);

            character = characters[randomIndex];
            anim = animators[randomIndex];
        }
    }
    public void SetAnimBool(string animName, bool value)
    {
        anim.SetBool(animName, value);     
    }
    public void SetAnimTrigger(string animName)
    {
        anim.SetTrigger(animName);
    }
    public void SetAnimFloat(string animName, float value)
    {
        anim.SetFloat(animName, value);
    }
    public void SetAnimInt(string animName, int value)
    {
        anim.SetInteger(animName, value);
    }
    public enum Character { Girl_Asian, Man_Black, Man_Blonde, Man_Brunette, Random = -1 }
}

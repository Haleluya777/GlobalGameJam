using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterMaker
{
    public List<Sprite> Hairs = new List<Sprite>();
    public List<Sprite> Faces = new List<Sprite>();
    public List<Sprite> Bodies = new List<Sprite>();
    public List<Sprite> Pants = new List<Sprite>();

    [Header("Character Template")]
    public GameObject template;

    [Header("Witness Face")]
    [SerializeField] private Sprite witnessFace;

    public GameObject MakeWitness(GameObject template)
    {
        GameManager.instance.canvasManager.targetSprites.Clear();

        //스프라이트 변경
        var _face = template.transform.GetChild(0).GetComponent<Image>();
        var _pants = template.transform.GetChild(1).GetComponent<Image>();
        var _body = template.transform.GetChild(2).GetComponent<Image>();
        var _hair = template.transform.GetChild(3).GetComponent<Image>();

        _face.sprite = witnessFace;
        _pants.sprite = Pants[Random.Range(0, Pants.Count)];
        _body.sprite = Bodies[Random.Range(0, Bodies.Count)];
        _hair.sprite = Hairs[Random.Range(0, Hairs.Count)];

        //클릭 시 이벤트 추가.
        var character = template.GetComponent<Character>();

        character.InitClickEvent(true);

        GameManager.instance.canvasManager.targetSprites.Add(witnessFace);
        GameManager.instance.canvasManager.targetSprites.Add(_pants.sprite);
        GameManager.instance.canvasManager.targetSprites.Add(_body.sprite);
        GameManager.instance.canvasManager.targetSprites.Add(_hair.sprite);

        GameManager.instance.canvasManager.SetTargetImg();

        //template.name = "Target";

        return template;
    }

    public GameObject MakeCharacter(GameObject template)
    {
        var _face = template.transform.GetChild(0).GetComponent<Image>();
        var _pants = template.transform.GetChild(1).GetComponent<Image>();
        var _body = template.transform.GetChild(2).GetComponent<Image>();
        var _hair = template.transform.GetChild(3).GetComponent<Image>();

        _face.sprite = Faces[Random.Range(0, Faces.Count)];
        _pants.sprite = Pants[Random.Range(0, Pants.Count)];
        _body.sprite = Bodies[Random.Range(0, Bodies.Count)];
        _hair.sprite = Hairs[Random.Range(0, Hairs.Count)];

        var character = template.GetComponent<Character>();

        character.InitClickEvent();

        return template;
    }
}

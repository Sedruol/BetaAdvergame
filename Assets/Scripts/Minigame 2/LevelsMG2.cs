using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Level MG2", menuName = "LevelsMG2")]
public class LevelsMG2 : ScriptableObject
{
    public enum TypeObjects
    {
        Salida,
        Lava,
        Muro,
        Caja,
        Nube,
        Agua
    }//nombres de los objetos editables
    public enum RuleObjects
    {
        Ganar,
        Perder,
        Detiene,
        Empujable,
        Flotar,
        Destruir
    }//nombres de los valores que pueden tener los objetos editables
    [SerializeField] private TextAsset textAsset;//archivo que contiene la matriz del nivel
    [SerializeField] private List<TypeObjects> namesObjects;//almacena los nombres de los objetos editables
    [SerializeField] private List<RuleObjects> namesValue;//almacena los nombres de los valores que pueden tener los objetos editables
    [SerializeField] private List<int> codeValues;//almacena el valor que representa cada objeto editable en la matriz
    public TextAsset GetTextAsset { get { return textAsset; } }
    public List<TypeObjects> GetNamesObjects { get { return namesObjects; } }
    public List<RuleObjects> GetNamesValues { get { return namesValue; } }
    public List<int> GetCodeValues { get { return codeValues; } }
    public List<int> SetCodeValues(List<int> _codeValues) { return this.codeValues = _codeValues; }
}
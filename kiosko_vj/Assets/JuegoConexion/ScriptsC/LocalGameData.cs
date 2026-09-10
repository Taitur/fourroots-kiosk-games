using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PerfilData
{
    public string nombre;
    public string puesto;
    public string departamento;
    public string skills;
    public List<DudaEntry> dudas = new List<DudaEntry>();
}

[Serializable]
public class DudaEntry
{
    public string id;
    public string texto;
    public string estado; // "EN_MATCH" | "RESUELTA"
    public MatchInfo match;
}

[Serializable]
public class MatchInfo
{
    public string dudaId;
    public string nombreQuienResponde;
    public string departamentoQuienResponde;
    public string motivoSkill;
    public bool hiloAbierto;
}

/// <summary>
/// Persistencia local (JSON en Application.persistentDataPath). Sin backend:
/// cumple el requisito "todo local" para el prototipo.
/// </summary>
public static class LocalGameData
{
    private const string FileName = "perfil_oficina.json";
    private static PerfilData _perfil;

    public static PerfilData Perfil
    {
        get
        {
            if (_perfil == null) Load();
            return _perfil;
        }
    }

    private static string PathOnDisk => Path.Combine(Application.persistentDataPath, FileName);

    public static void Load()
    {
        if (File.Exists(PathOnDisk))
        {
            try { _perfil = JsonUtility.FromJson<PerfilData>(File.ReadAllText(PathOnDisk)); }
            catch { _perfil = new PerfilData(); }
        }
        else
        {
            _perfil = new PerfilData();
        }
    }

    public static void Save()
    {
        File.WriteAllText(PathOnDisk, JsonUtility.ToJson(_perfil, true));
    }

    public static bool TieneDudasPendientes()
    {
        foreach (var d in Perfil.dudas)
            if (d.match != null && !d.match.hiloAbierto) return true;
        return false;
    }

    public static DudaEntry UltimaDudaConMatchNoAbierto()
    {
        for (int i = Perfil.dudas.Count - 1; i >= 0; i--)
            if (Perfil.dudas[i].match != null && !Perfil.dudas[i].match.hiloAbierto)
                return Perfil.dudas[i];
        return null;
    }

    public static void BorrarTodo()
    {
        _perfil = new PerfilData();
        if (File.Exists(PathOnDisk)) File.Delete(PathOnDisk);
    }
}

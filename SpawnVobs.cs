using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;
using System.Threading;

public class SpawnVobs : MonoBehaviour
{
    public Transform parentCol;
    public Transform parentNoCol;
    public Transform parentDynamic;
    public Transform parentInteractable;
    public DialogueParser lsdf;
    public List<Vob> vobs = new List<Vob>();
    public bool spawn;
    public bool save;
    public GameObject[] spawnedObj;
    void Start()
    {
        if (save)
            for (int i = 0; i < lsdf.vobs.Count; i++)
            {
                Vob vob = lsdf.vobs[i];
                if (vob != null && vob.name.Length > 0 && !vob.Equals(vobs))
                {
                    vobs.Add(vob);
                }
            }
        if (spawn)
        {
            if (spawnedObj.Length == 0)
                spawnedObj = new GameObject[vobs.Count];
            Spawn();
        }
    }

    void DuplicateCheck()
    {
        for (int x = 0; x < vobs.Count - 1; x++)
        {
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            double x1 = double.Parse(vobs[x].posX, formatter);
            double y1 = double.Parse(vobs[x].posY, formatter);
            double z1 = double.Parse(vobs[x].posZ, formatter);

            double x2 = double.Parse(vobs[x + 1].posX, formatter);
            double y2 = double.Parse(vobs[x + 1].posY, formatter);
            double z2 = double.Parse(vobs[x + 1].posZ, formatter);
            if (x1 == x2 && y1 == y2 && z1 == z2)
            {
                vobs.RemoveAt(x + 1);
            }
        }
    }
    void Spawn()
    {
        //DuplicateCheck();
        for (int i = 0; i < vobs.Count; i++)
        {
            if (spawnedObj[i] is not null) continue;
            GameObject obj = Resources.Load("Mobs/" + vobs[i].name) as GameObject;
            if (obj)
            {
                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                double x = double.Parse(vobs[i].posX, formatter);
                double y = double.Parse(vobs[i].posY, formatter);
                double z = double.Parse(vobs[i].posZ, formatter);
                Matrix4x4 mat = HexToMat(vobs[i].rot);
                float[,] mat1 = new float[3, 3];
                mat1[0, 0] = HexToFloat(vobs[i].rot.Substring(0, 8));
                mat1[1, 0] = HexToFloat(vobs[i].rot.Substring(8, 8));
                mat1[2, 0] = HexToFloat(vobs[i].rot.Substring(16, 8));
                mat1[0, 1] = HexToFloat(vobs[i].rot.Substring(24, 8));
                mat1[1, 1] = HexToFloat(vobs[i].rot.Substring(32, 8));
                mat1[2, 1] = HexToFloat(vobs[i].rot.Substring(40, 8));
                mat1[0, 2] = HexToFloat(vobs[i].rot.Substring(48, 8));
                mat1[1, 2] = HexToFloat(vobs[i].rot.Substring(56, 8));

                mat1[2, 2] = HexToFloat(vobs[i].rot.Substring(64, 8));
                Collider collider = obj.GetComponent<Collider>();
                InteractiveBed bed = obj.GetComponent<InteractiveBed>();
                InteractiveChair chair = obj.GetComponent<InteractiveChair>();
                CandleScript candle = obj.GetComponent<CandleScript>();
                Chest chest = obj.GetComponent<Chest>();
                Transform targetTransform = parentNoCol;
                if (collider)
                {
                    Instantiate(obj, new Vector3((float)(-x / 100) - 722f, (float)(y / 100) - 43.8f, (float)(-z / 100) - 116f), Matrix3ToQuat(mat1), parentCol);
                }
                if (!obj.isStatic)
                    targetTransform = parentDynamic;
                if (bed || chair || candle || chest)
                    targetTransform = parentInteractable;
                Instantiate(obj, new Vector3((float)(-x / 100) - 722f, (float)(y / 100) - 43.8f, (float)(-z / 100) - 116f), Matrix3ToQuat(mat1), targetTransform);
                spawnedObj[i] = obj;
            }
            else
            {
                GameObject obj2 = Resources.Load("Items/WorldItems/" + vobs[i].name) as GameObject;
                if (obj2)
                {
                    IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                    double x = double.Parse(vobs[i].posX, formatter);
                    double y = double.Parse(vobs[i].posY, formatter);
                    double z = double.Parse(vobs[i].posZ, formatter);
                    Matrix4x4 mat = HexToMat(vobs[i].rot);
                    float[,] mat1 = new float[3, 3];
                    mat1[0, 0] = HexToFloat(vobs[i].rot.Substring(0, 8));
                    mat1[1, 0] = HexToFloat(vobs[i].rot.Substring(8, 8));
                    mat1[2, 0] = HexToFloat(vobs[i].rot.Substring(16, 8));
                    mat1[0, 1] = HexToFloat(vobs[i].rot.Substring(24, 8));
                    mat1[1, 1] = HexToFloat(vobs[i].rot.Substring(32, 8));
                    mat1[2, 1] = HexToFloat(vobs[i].rot.Substring(40, 8));
                    mat1[0, 2] = HexToFloat(vobs[i].rot.Substring(48, 8));
                    mat1[1, 2] = HexToFloat(vobs[i].rot.Substring(56, 8));
                    mat1[2, 2] = HexToFloat(vobs[i].rot.Substring(64, 8));
                    Instantiate(obj2, new Vector3((float)(-x / 100) - 722f, (float)(y / 100) - 43.8f, (float)(-z / 100) - 116f), QuaternionFromMatrix(mat), parentNoCol);
                    spawnedObj[i] = obj2;
                }
            }

        }
    }
    public string SwapEndianness(string hex)
    {

        string newHex = "";
        newHex += hex[6];
        newHex += hex[7];
        newHex += hex[4];
        newHex += hex[5];
        newHex += hex[2];
        newHex += hex[3];
        newHex += hex[0];
        newHex += hex[1];
        return newHex;
    }
    float HexToFloat(string hex)
    {
        string hexString = SwapEndianness(hex);
        Int32 IntRep = Int32.Parse(hexString, NumberStyles.HexNumber);
        // Integer to Byte[] and presenting it for float conversion
        float myFloat = BitConverter.ToSingle(BitConverter.GetBytes(IntRep), 0);
        // There you go
        return myFloat;
    }

    Matrix4x4 HexToMat(string hex)
    {
        Vector4 m1 = new Vector4(HexToFloat(hex.Substring(0, 8)), HexToFloat(hex.Substring(24, 8)), HexToFloat(hex.Substring(48, 8)), 0);
        Vector4 m2 = new Vector4(HexToFloat(hex.Substring(8, 8)), HexToFloat(hex.Substring(32, 8)), HexToFloat(hex.Substring(56, 8)), 0);
        Vector4 m3 = new Vector4(HexToFloat(hex.Substring(16, 8)), HexToFloat(hex.Substring(40, 8)), HexToFloat(hex.Substring(64, 8)), 0);
        return new Matrix4x4(m1, m2, m3, new Vector4(0, 0, 0, 0));
    }

    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
        q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
        q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
        q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
        return q;
    }


    Quaternion Matrix3ToQuat(float[,] mat)
    {
        float dig = mat[0, 0] + mat[1, 1] + mat[2, 2];
        Quaternion qt = new Quaternion();
        float kf;
        if (dig > 0)
        {
            kf = Mathf.Sqrt(dig + 1.0f);
            qt[3] = kf * 0.5f;
            kf = 0.5f / kf;
            qt[0] = (mat[1, 2] - mat[2, 1]) * kf;
            qt[1] = (mat[2, 0] - mat[0, 2]) * kf;
            qt[2] = (mat[0, 1] - mat[1, 0]) * kf;
            return qt;
        }
        int k = 0;
        if (mat[1, 1] > mat[0, 0])
            k = 1;
        if (mat[2, 2] > mat[k, k])
            k = 2;
        int[] r = { 1, 2, 0 };
        kf = Mathf.Sqrt(mat[k, k] - (mat[r[r[k]], r[r[k]]] + mat[r[k], r[k]]) + 1.0f);
        qt[k] = kf * 0.5f;
        if (kf != 0)
            kf = 0.5f / kf;
        qt[3] = (mat[r[k], r[r[k]]] - mat[r[r[k]], r[k]]) * kf;
        qt[r[k]] = (mat[k, r[k]] + mat[r[k], k]) * kf;
        qt[r[r[k]]] = (mat[k, r[r[k]]] + mat[r[r[k]], k]) * kf;
        return qt;
    }
}

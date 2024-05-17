using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Globalization;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Dialoge
{
    public int talker;
    public string audioClip;
    public string Text;

   


}
[System.Serializable]
public class Vob
{
    public string name = string.Empty;
    public string rot;
    public string posX;
    public string posY;
    public string posZ;

    public bool Equals(List<Vob> vobList)
    {

        for (int i = 0; i < vobList.Count; i++)
        {
            Vob other = vobList[i];
            if (name == other.name && rot == other.rot && posX == other.posX && posY == other.posY && posZ == other.posZ)
                return true;
        }
        return false;
    }

}
//56022 100277 163117 185246 217442 263280 281455 312450 341297 384951 412897 436856 502622 528170 570521
public class DialogueParser : MonoBehaviour
{
   
    public int rot;

    private string text = "";

   public List<Vob> vobs = new List<Vob>();

    void Awake()
    {
        using (StreamReader sr = File.OpenText("Dia/fff.txt"))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                text += s +"+";
            }
        }
        int space = -1;
        bool LineEnd = false;
        bool LineEnd2 = false;
        bool LineEnd3 = false;
        bool LineEnd4 = false;
        bool LineEnd5 = false;
        int Spaces = 0;
        bool overText = false;
        bool overText2 = false;
        bool overText3 = false;
        bool overText4 = false;
        bool overText5 = false;
        bool nameTxt = false;
        bool nameTxt2 = false;
        bool nameTxt3 = false;
        bool nameTxt4 = false;
        int q = 0;
        for (int i = 0; i < text.Length; i++)
        {
            
            string chr = text[i].ToString();
            if (chr == "z")
            {
                
                if (i< text.Length -1&& text[i + 1].ToString() == "C" &&
                    i < text.Length - 2 && text[i + 2].ToString() == "V" &&
                    i < text.Length - 3 && text[i + 3].ToString() == "o" &&
                    i < text.Length - 4 && text[i + 4].ToString() == "b" &&
                    i < text.Length - 5 && text[i + 5].ToString() == " ")
                {


                    nameTxt = false;
                    vobs.Add(new Vob());
                    space++;
                   
                }
                
            }
            
                if (chr == "t")
            {
                    Spaces = 0;
                    overText = false;
                    if (i < text.Length - 1 && text[i + 1].ToString() == "r" &&
                    i < text.Length - 2 && text[i + 2].ToString() == "a" &&
                    i < text.Length - 3 && text[i + 3].ToString() == "f" &&
                    i < text.Length - 4 && text[i + 4].ToString() == "o" &&
                    i < text.Length - 5 && text[i + 5].ToString() == "O" &&
                    i < text.Length - 6 && text[i + 6].ToString() == "S" &&
                    i < text.Length - 7 && text[i + 7].ToString() == "T" &&
                    i < text.Length - 8 && text[i + 8].ToString() == "o" &&
                    i < text.Length - 9 && text[i + 9].ToString() == "W" &&
                    i < text.Length - 10 && text[i + 10].ToString() == "S" &&
                    i < text.Length - 11 && text[i + 11].ToString() == "P" &&
                    i < text.Length - 12 && text[i + 12].ToString() == "o")
                {

                    LineEnd = false;
                    q = 0;
                }
            }
            if (chr == "t")
            {
                overText5 = false;
                if (i < text.Length - 1 && text[i + 1].ToString() == "r" &&
                i < text.Length - 2 && text[i + 2].ToString() == "a" &&
                i < text.Length - 3 && text[i + 3].ToString() == "f" &&
                i < text.Length - 4 && text[i + 4].ToString() == "o" &&
                i < text.Length - 5 && text[i + 5].ToString() == "O" &&
                i < text.Length - 6 && text[i + 6].ToString() == "S" &&
                i < text.Length - 7 && text[i + 7].ToString() == "T" &&
                i < text.Length - 8 && text[i + 8].ToString() == "o" &&
                i < text.Length - 9 && text[i + 9].ToString() == "W" &&
                i < text.Length - 10 && text[i + 10].ToString() == "S" &&
                i < text.Length - 11 && text[i + 11].ToString() == "R" &&
                i < text.Length - 12 && text[i + 12].ToString() == "o")
                {

                    LineEnd5 = false;
                    nameTxt4 = true;
                }
            }
            if (space > -1)
            {
                if (chr == "p")
            {
                overText2 = false;
                if (i < text.Length - 1 && text[i + 1].ToString() == "r")
                {
                    nameTxt = true;
                    LineEnd2 = false;
                }
            }
            if (chr == "v")
            {
                overText3 = false;
                if (i < text.Length - 1 && text[i + 1].ToString() == "o" &&
                   i < text.Length - 2 && text[i + 2].ToString() == "b" &&
                   i < text.Length - 3 && text[i + 3].ToString() == "N") { 
                    nameTxt2 = true;
                    LineEnd3 = false;
                }
            }
            if (chr == "v")
            {
                overText4 = false;
                if (i < text.Length - 1 && text[i + 1].ToString() == "i" &&
                    i < text.Length - 2 && text[i + 2].ToString() == "s" &&
                    i < text.Length - 3 && text[i + 3].ToString() == "u" &&
                    i < text.Length - 4 && text[i + 4].ToString() == "a" &&
                    i < text.Length - 5 && text[i + 5].ToString() == "l" &&
                    i < text.Length - 6 && text[i + 6].ToString() == "=")
                {
                    nameTxt3 = true;
                    LineEnd4 = false;
                }
            }
            
                if (text[i] == ':' && q < 2)
                    {
                        overText = true;
                        q++;
                    }
                if (text[i] == ' ' && q < 2)
                {
                    Spaces++;
                }
                    
                    if (chr == "+") LineEnd = true;
                    if (!LineEnd)
                    if (overText&& text[i] != ':' && text[i] != ' ')
                    {
                        if (Spaces == 0)
                            vobs[space].posX += chr;
                        if (Spaces == 1)
                            vobs[space].posY += chr;
                        if (Spaces == 2)
                            vobs[space].posZ += chr;
                    }
                if (false)
                {
                    if (text[i] == ':')
                    {
                        overText2 = true;
                    }
                    if (chr == "+") LineEnd2 = true;
                    if (!LineEnd2)
                        if (overText2 && text[i] != ':' && text[i] != '.' && text[i] != '+')
                    vobs[space].name += chr;
                }
                if (false)
                {
                    if (text[i] == ':')
                    {
                        overText3 = true;
                    }
                    if (chr == "+") LineEnd3 = true;
                    if (!LineEnd3)
                        if (overText3 && text[i] != ':' && text[i] != '.' && text[i] != '+')
                            vobs[space].name += chr;
                }
                if (nameTxt3)
                {
                    if (text[i] == ':')
                    {
                        overText4 = true;
                    }
                    if (chr == ".") LineEnd4 = true;
                    if (chr == "+") LineEnd4 = true;
                    if (!LineEnd4)
                        if (overText4 && text[i] != '.' && text[i] != ':' && text[i] != '+')
                            vobs[space].name += chr;
                }
                if (nameTxt4)
                {
                    if (text[i] == ':')
                    {
                        overText5 = true;
                    }
                    if (chr == ".") LineEnd5 = true;
                    if (chr == "+") LineEnd5 = true;
                    if (!LineEnd5)
                        if (overText5 && text[i] != '.' && text[i] != ':' && text[i] != '+')
                            vobs[space].rot += chr;
                }
            }
        }
    }




    /*
       using (StreamReader sr = File.OpenText("Dia/fff.txt"))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                text += s +"+";
            }
        }
        int space = -1;
bool LineEnd = false;
bool txtG = false;
bool overText = false;
int q = 0;
for (int i = 0; i < text.Length; i++)
{

    string chr = text[i].ToString();
    if (chr == "A")
    {

        txtG = false;
        if (i < text.Length - 1 && text[i + 1].ToString() == "I" &&
            i < text.Length - 2 && text[i + 2].ToString() == "_" &&
            i < text.Length - 3 && text[i + 3].ToString() == "O" &&
            i < text.Length - 4 && text[i + 4].ToString() == "u" &&
            i < text.Length - 5 && text[i + 5].ToString() == "t" &&
            i < text.Length - 6 && text[i + 6].ToString() == "p" &&
            i < text.Length - 7 && text[i + 7].ToString() == "u" &&
            i < text.Length - 8 && text[i + 8].ToString() == "t" &&
            i < text.Length - 9 && text[i + 9].ToString() == "(")
        {

            q = 0;
            LineEnd = false;
            diaTxt.Add(new Dialoge());
            space++;

            if (i < text.Length - 10 && text[i + 10].ToString() == "s")
                diaTxt[space].talker = 1;
            if (i < text.Length - 10 && text[i + 10].ToString() == "o")
                diaTxt[space].talker = 0;
        }

    }
    if (space > -1)
    {
        if (text[i] == '"' && q < 2)
        {
            overText = !overText;
            q++;
        }
        if (overText)
        {
            if (text[i] != '"')
                diaTxt[space].audioClip += chr;
        }
        if (chr == "/") txtG = true;
        if (chr == "+") LineEnd = true;
        if (!LineEnd && txtG && chr != "/")
            diaTxt[space].Text += chr;
    }
}
*/
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaClient
{
    LuaEnv luaEnv;
    public void Init()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(LavenderLuaLoader);
        luaEnv.DoString("require 'Core/LuaGameInstance'");
        luaEnv.DoString("LuaGameInstance.Init()");
    }
    
    public void Update()
    {
        luaEnv.DoString("LuaGameInstance.Update()");
    }

    //回调方法
    //可以在回调方法自定义路径
    private byte[] LavenderLuaLoader(ref string fileName)
    {
        //Application.dataPath 表示Assets路径
        //定义lua路径
        string luaPath = Application.dataPath + "/Script/Lua/" + fileName + ".lua";

        //读取lua路径中指定lua文件内容
        string strLuaContent = File.ReadAllText(luaPath);

        byte[] byArrayReturn = null; //返回数据
        //数据类型转换
        byArrayReturn = System.Text.Encoding.UTF8.GetBytes(strLuaContent);

        return byArrayReturn;
    }

    public void Excute(string command)
    {
        luaEnv.DoString(command);
    }
}

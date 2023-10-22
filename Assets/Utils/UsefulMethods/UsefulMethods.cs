using System;
using UnityEngine;
using MyBox;

public static class UsefulMethods 
{
    public static T GetRandomFromEnum<T>() where T : Enum
    {
        // Obtiene todos los valores del enum y los coloca en un array.
        T[] valoresEnum = (T[])Enum.GetValues(typeof(T));
        System.Random random = new System.Random();
        // Genera un número aleatorio para seleccionar un índice.
        int indiceAleatorio = random.Next(0, valoresEnum.Length);

        // Devuelve el valor aleatorio del enum.
        return valoresEnum[indiceAleatorio];
    }

    public static Vector3 PositionInTheCenterOfTheScreen()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 centerInWorldSpace = Camera.main.ScreenToWorldPoint(screenCenter);
        return centerInWorldSpace.SetZ(0f);
    }
}
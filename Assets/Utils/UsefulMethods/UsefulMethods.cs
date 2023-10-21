using System;
public static class UsefulMethods 
{
    public static T GetRandomFromEnum<T>() where T : Enum
    {
        // Obtiene todos los valores del enum y los coloca en un array.
        T[] valoresEnum = (T[])Enum.GetValues(typeof(T));
        Random random = new Random();
        // Genera un número aleatorio para seleccionar un índice.
        int indiceAleatorio = random.Next(0, valoresEnum.Length);

        // Devuelve el valor aleatorio del enum.
        return valoresEnum[indiceAleatorio];
    }
}
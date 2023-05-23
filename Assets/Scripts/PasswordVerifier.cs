using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using UnityEngine;

public class PasswordVerifier : MonoBehaviour
{
    public static PasswordVerifier instance;
    private const int SaltSize = 16;
    private const int HashSize = 20;
    private const int Iterations = 10000;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Verifies if the password meets the requirements
    public bool VerifyPasswordLength(string password)
    {
        if (password.Length >= 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool VerifyPasswordPattern(string password)
    {
        // Regular expression pattern to match the password requirements
        string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$";

        if (Regex.IsMatch(password, pattern))
        {
            Debug.Log("Password verification successful.");
            return true;
        }
        else
        {
            Debug.Log("Password verification failed.");
            return false;
        }
    }

    // Verifies the provided password against the stored hash
    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var derivedBytes = new Rfc2898DeriveBytes(password, storedSalt, Iterations))
        {
            byte[] computedHash = derivedBytes.GetBytes(HashSize);

            // Compare the computed hash with the stored hash
            if (AreByteArraysEqual(computedHash, storedHash))
            {
                Debug.Log("Password verification successful.");
                return true;
            }
            else
            {
                Debug.Log("Password verification failed.");
                return false;
            }
        }
    }

    // Compares two byte arrays for equality
    private bool AreByteArraysEqual(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }

        return true;
    }
}

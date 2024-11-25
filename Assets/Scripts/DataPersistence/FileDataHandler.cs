using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    string _dataDirPath;
    string _dataFileName;

    bool _useEncrytion;
    readonly string _encryptionCodeWord = "deadrobotzombiecopfromouterspace";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this._dataDirPath = dataDirPath;
        this._dataFileName = dataFileName;
        this._useEncrytion = useEncryption;
    }

    /// <summary>
    /// Loads the game data from a file.
    /// </summary>
    /// <returns>The loaded GameData object, or null if loading fails.</returns>
    public GameData Load()
    {
        // Construct the full path to the data file
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        GameData loadedData = null;

        // Check if the data file exists
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad;

                // Open the file and read its contents
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Decrypt the data if encryption is enabled
                if (_useEncrytion)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // Deserialize the data to a GameData object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                // Log an error if loading fails
                Debug.LogError("Failed to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    /// <summary>
    /// Saves the game data to a file.
    /// </summary>
    /// <param name="data">The GameData object to save.</param>
    public void Save(GameData data)
    {
        // Construct the full path to the data file
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        try
        {
            // Create the directories if they don't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Convert the GameData object to a JSON string
            string dataToStore = JsonUtility.ToJson(data, true);

            // Encrypt the data if encryption is enabled
            if (_useEncrytion)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // Open the file and write the data to it
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            // Log an error if saving fails
            Debug.LogError("Failed to save data to file: " + fullPath + "\n" + e);
        }
    }

    /// <summary>
    /// Encrypts or decrypts the provided string using the XOR encryption
    /// algorithm with the _encryptionCodeWord string.
    /// </summary>
    /// <param name="data">The string to encrypt or decrypt.</param>
    /// <returns>The encrypted or decrypted string.</returns>
    string EncryptDecrypt(string data)
    {
        string modifiedData = string.Empty;

        // Loop through each character in the string and XOR it with the corresponding
        // character in the encryption code word.
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        }

        return modifiedData;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DownloadImageScreenManager : MonoBehaviour
{
    [System.Serializable]
    public class ImageItem
    {
        public Texture2D image;      // Image itself
        public string imageName;     // Name for saved file
    }

    public Transform contentParent; // ScrollView Content
    public GameObject imagePrefab;  // Prefab with RawImage + Download Button
    public ImageItem[] images;      // Array of images to show

    void Start()
    {
        // Populate ScrollView with images
        foreach (var item in images)
        {
            GameObject obj = Instantiate(imagePrefab, contentParent);
            RawImage rawImage = obj.GetComponentInChildren<RawImage>();
            rawImage.texture = item.image;

            Button btn = obj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => DownloadImage(item));
        }
    }

    void DownloadImage(ImageItem item)
    {
        string path = Path.Combine(Application.persistentDataPath, item.imageName + ".png");
        byte[] bytes = item.image.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log("Image downloaded: " + path);
    }
}
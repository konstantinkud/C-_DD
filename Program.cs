using System;
using System.Xml;
using System.IO;

class Program
{
    static void Main()
    {
        string filePath = "./tolstoj_text.fb2";
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
        
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("fb", "http://www.gribuser.ru/xml/fictionbook/2.1");
        
            XmlNodeList paragraphNodes = doc.SelectNodes("//fb:p", nsManager);
            
            string allText = "";
            foreach (XmlNode paragraphNode in paragraphNodes)
            {
                allText += paragraphNode.InnerText + " ";
            }
            
            string[] words = allText.Split(new char[] { ' ', ',', '.', '!', '?', ':', ';', '-', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (wordCount.ContainsKey(word))
                {
                    wordCount[word]++;
                }
                else
                {
                    wordCount[word] = 1;
                }
            }
            
            var sortedWordCount = wordCount.OrderByDescending(pair => pair.Value);
        
            using (StreamWriter writer = new StreamWriter("word_count.txt"))
            {
                foreach (var pair in sortedWordCount)
                {
                    writer.WriteLine($"{pair.Key}\t{pair.Value}");
                }
                Console.WriteLine("Count saved to word_count.txt");
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }
    }
}
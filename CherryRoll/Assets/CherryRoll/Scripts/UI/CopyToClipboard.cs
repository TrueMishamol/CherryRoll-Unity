using UnityEngine;

public static class CopyToClipboard {

    public static void Copy(this string text) {
        TextEditor textEditor = new TextEditor();
        textEditor.text = text;
        textEditor.SelectAll();
        textEditor.Copy();
    }
}

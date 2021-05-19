using UnityEngine;
using Yarn.Unity;

public class DialogueProgresser : MonoBehaviour
{
    private InMemoryVariableStorage yarnMemmory;
    public UnityEngine.Events.UnityEvent onCollide;

    private void Awake()
    {
        yarnMemmory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
    }

    public void OnTriggerEnter(Collider other)
    {
        yarnMemmory.SetValue("$cutsceneRunning", true);
        onCollide.Invoke();
    }
}
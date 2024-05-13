/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using Sirenix.OdinInspector;
using UnityEngine;


///<summary>
///InspctorToolExample description
////</summary>
public class InspctorToolExample : MonoBehaviour
{
#if ODIN_INSPECTOR == false
    [HorizontalLine(Thickness = 2, Padding = 10)]
#endif
    [BoxGroup("Features")]
    [SerializeField] [Tooltip("Name of Character")]
    private string _name = string.Empty;
    [Space(10f)]
#if ODIN_INSPECTOR == false
    [Note("The description should be relevant information where the character is from")]
#endif
    [BoxGroup("Features")]
    [SerializeField]
    [TextArea] private string _description = string.Empty;
    
    [InfoBox("Add the audio files in that format...")]
    
    #if ODIN_INSPECTOR == false
    [HorizontalLine(Thickness = 4, Padding = 20)]
#endif
    [BoxGroup("Audio")]
    [SerializeField] [Tooltip("Death audio clip")]
    private AudioClip _deathAudioClip;
    [BoxGroup("Audio")]
    [SerializeField] [Tooltip("Angry audio clip")]
    private AudioClip _angryAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class CombatController : MonoBehaviour 
{	
	public BasicMovementController movement;	
	public int playerId = 0;
	/* public Player player
	{
		get 
		{	
			//if(playerControllerId == -1)
				return ReInput.players.GetPlayer(playerId);
			//else
			//	return ReInput.players.GetPlayer(playerControllerId);
		}			
	} */

	public AnimationClip Light, Medium, Heavy;
	Animator anim;
	AnimatorOverrideController AOcontroller;
	AnimationClipOverrides AOclips;

	
	PlayableGraph playGraph;
	AnimationMixerPlayable mixPlayer;
	AnimatorControllerPlayable ctrlPlayable;

	AnimationClipPlayable[] clipPlayables;
	public  Combo[] combos;

	public RuntimeAnimatorController controller;
	public float weight;
	Queue<AnimationClip> clips;
	public AnimationClip[] clipArray;

	bool blending = false;
	float blendDuration = .5f;
	float blendTime =0;
	int index = 0;
	bool queued;

	
	
	

	void Start()
	{	
		// clips = new Queue<AnimationClip>();	
		// clipPlayables = new AnimationClipPlayable[5];
		 anim = GetComponent<Animator>();
		 AOcontroller = new AnimatorOverrideController(anim.runtimeAnimatorController);
		 anim.runtimeAnimatorController = AOcontroller;

		 AOclips = new AnimationClipOverrides(AOcontroller.overridesCount);
		 AOcontroller.GetOverrides(AOclips);

		 AOclips["Light"] = Light;
		 AOclips["Medium"] = Medium;
		 AOclips["Heavy"] = Heavy;

		// playGraph = PlayableGraph.Create();
		// //var playQueuePlayable = ScriptPlayable<PlayQueuePlayable>.Create(playGraph);

        // //var playQueue = playQueuePlayable.GetBehaviour();

		// var playableOutput = AnimationPlayableOutput.Create(playGraph, "Animation", anim);
		// mixPlayer = AnimationMixerPlayable.Create(playGraph, 4);
        
        // playableOutput.SetSourcePlayable(mixPlayer);

		// ctrlPlayable = AnimatorControllerPlayable.Create(playGraph, controller);

		// playGraph.Connect(ctrlPlayable, 0, mixPlayer, index);

		// playGraph.Play();
		// AnimationPlayableUtilities.PlayAnimatorController(anim,controller, out playGraph);

		//StartCoroutine(PlayQueue () );
	}

	void Update() 
	{
		// QueueAnimations();
		// if(clips.Count > 0)
		// {
		// 	StartCoroutine(PlayQueue());
		// }

		foreach (var combo in combos)
		{
			/* combo.Init(player);
			if(combo.Check(player)) */
			{				
				AOcontroller["Special"] = combo.specialMove;
				anim.SetTrigger("Special");
				print("executing " + combo.name.ToString());							
			}				
		}		
		
	}
 
	IEnumerator  PlayQueue () 
	{
		if(clips.Count == 0) yield break;
		
		for (int i = 0; i <= clips.Count ; i++)
		{			
		
			print("Queue size is: " + clips.Count.ToString());		
		

			
			//print("starting queue");	
			// if(player.GetButtonDown("Light")) anim.SetTrigger("VSlash_01");
			// if(player.GetButtonDown("Medium")) anim.SetTrigger("VSlash2HSlash");
			// if(player.GetButtonDown("Heavy")) anim.SetTrigger("FireSwd01");

		
			//yield return new WaitForSeconds(tick);		

		
					
			clipPlayables[i] = AnimationPlayableUtilities.PlayClip(anim, clips.Dequeue(), out playGraph);
			print("playing animation");		
			clipArray = clips.ToArray();	
			movement.isPaused = true;			
			yield return new WaitForSeconds(clipPlayables[i].GetAnimationClip().length);
			print("animation has finished");
			
			movement.isPaused = false;
			clipPlayables[i].Destroy();
			playGraph.Play();
		

			yield return null;
		}
		// if(clips.Count > 0 )
		// {			
		// 	blending = true;
		// 	//clipPlayable = AnimationClipPlayable.Create(playGraph, clips.Dequeue());			
		// 	//playGraph.Connect(clipPlayable, 0, mixPlayer, index+1);
		// 	index++;
		// }else
		// {
		// 	mixPlayer.SetInputWeight(0, 1);
		// 	index = 0;
		// 	//clipPlayable.Destroy();
		// }

		// if(clipPlayable.IsValid() && clipPlayable.IsDone()) 
		// {
		// 	mixPlayer.SetInputWeight(0, 1);
		// 	playGraph.Disconnect(clipPlayable,index+1);
		// 	clipPlayable.Destroy();
		// }

		// //if(blending == false) return;

		// blendTime += Time.deltaTime;
		// weight = blendTime / blendDuration;
		// weight = Mathf.Clamp01(weight);	
		// if(index == 0) return;	
		// mixPlayer.SetInputWeight(index-1, 1.0f-weight);
		// mixPlayer.SetInputWeight(index, weight);

		// if(weight >= 1)
		// {
		// 	mixPlayer.SetInputWeight(0, 1);
		// 	blendTime = 0;
		// 	//blending = false;
		// 	//playGraph.Disconnect(clipPlayable,0);
		// }

		
		
		
	
	
	}

	void QueueAnimations()
	{
		foreach (var combo in combos)
		{
/* 			combo.Init(player);
			if(combo.Check(player)) */
			{				
				clips.Enqueue(combo.specialMove);
				//print("executing " + combo.name.ToString());							
			}
			clipArray = clips.ToArray();		
		}		
	}

	
	

	void OnDisable()

    {

        // Destroys all Playables and Outputs created by the graph.

        //playGraph.Destroy();

    }


}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) {}

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

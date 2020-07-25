using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class MoveSet : MonoBehaviour 
{	
	
	public int playerId = 0;
	/* public Player player
	{
		get 
		{	
			if(playerControllerId == -1)
				return ReInput.players.GetPlayer(playerId);
			else
				return ReInput.players.GetPlayer(playerControllerId);
		}			
	} */

	Animator anim;
	PlayableGraph playGraph;
	AnimationMixerPlayable mixPlayer;
	AnimatorControllerPlayable ctrlPlayable;

	AnimationClipPlayable clipPlayable;
	public  Combo[] combos;

	public RuntimeAnimatorController controller;
	public float weight;
	Queue<AnimationClip> clips;
	

	void Start()
	{	
		clips = new Queue<AnimationClip>();	
		anim = GetComponent<Animator>();	
		playGraph = PlayableGraph.Create();
		var playQueuePlayable = ScriptPlayable<PlayQueuePlayable>.Create(playGraph);

        var playQueue = playQueuePlayable.GetBehaviour();

		var playableOutput = AnimationPlayableOutput.Create(playGraph, "Animation", anim);
		mixPlayer = AnimationMixerPlayable.Create(playGraph, 2);
        
        playableOutput.SetSourcePlayable(mixPlayer);

		ctrlPlayable = AnimatorControllerPlayable.Create(playGraph, controller);

		playGraph.Connect(ctrlPlayable, 0, mixPlayer, 1);

		playGraph.Play();


	}
 
	void Update () 
	{

		//if(!isLocalPlayer) return;

		// if(player.GetButtonDown("Light")) anim.SetTrigger("VSlash_01");
		// if(player.GetButtonDown("Medium")) anim.SetTrigger("VSlash2HSlash");
		// if(player.GetButtonDown("Heavy")) anim.SetTrigger("FireSwd01");

		foreach (var combo in combos)
		{
			/* combo.Init(player);
			if(combo.Check(player)) */
			{
				//AnimationPlayableUtilities.PlayClip(anim, combo.specialMove, out playGraph);
				clips.Enqueue(combo.specialMove);
			}	
		}
		
		if(clips.Count > 0)
		{
			playGraph.Disconnect(clipPlayable,0);
			clipPlayable = AnimationClipPlayable.Create(playGraph, clips.Dequeue());
			playGraph.Connect(clipPlayable, 0, mixPlayer, 0);
		}

		weight = Mathf.Clamp01(weight);
		mixPlayer.SetInputWeight(0, 1.0f-weight);
		mixPlayer.SetInputWeight(1, weight);
		
	}

	void OnDisable()

    {

        // Destroys all Playables and Outputs created by the graph.

        playGraph.Destroy();

    }


}

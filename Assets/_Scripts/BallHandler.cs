using UnityEngine;
using System.Collections;

public class BallHandler : MonoBehaviour
{
    #region public
    [Header("Scripts")]
	public GameHandler gameHandler = null;
	public ScreenShake screenShake = null;
    public PaddlePunch paddlePunch = null;
	public UiHandler uiHandler = null;

    [Header("Puck/Ball Materials")]
	public Texture ballTextureWhite = null;
	public Texture ballTextureWhiteEmission = null;
	public Texture ballTextureBlue = null;
	public Texture ballTextureOrange = null;

    [Header("Trail Materials")]
	public Material trailMaterialWhite = null;
	public Material trailMaterialBlue = null;
	public Material trailMaterialOrange = null;

    [Space(5.0f)]
	public float ballSpeed = 25.0f;
	public float ballSpeedDefault = 25.0f;
	public float ballSpeedIncrease = 0.5f;
	public float randomBallSpeedZ = 15.0f;
	public float inGoalPosX = 21.0f;
    #endregion

    #region private
    private Animator cameraAnimator = null;

    private Rigidbody ballRigidbody = null;

	private Renderer ballMaterial = null;

	private TrailRenderer ballTrail = null;

	private float ballSpeedZ = 0.0f;
	private float lORr = 0.0f;
	#endregion

	private void Awake()
	{
        cameraAnimator = gameHandler.mainCamera.GetComponent<Animator>();

        StopAllCoroutines();
	}

	private void Start()
	{
		// rigidbody
		ballRigidbody = GetComponent<Rigidbody>();

		// material
		ballMaterial = GetComponent<Renderer>();
		SetBallTexture(ballTextureWhite);

		// trail
		ballTrail = GetComponent<TrailRenderer>();
		ballTrail.material = trailMaterialWhite;
    }

	private void Update()
	{
        switch (GameHandler.Instance.gameStates)
        {
            case GameHandler.GameStates.KickOff:
                StartCoroutine(KickOff());
                break;
            case GameHandler.GameStates.Playing:
                ballRigidbody.velocity = ballRigidbody.velocity.normalized * ballSpeed;
                break;
        }
	}

	// collisiondetection, particlecolor, trailcolor
	private void OnCollisionEnter(Collision collisionObject)
	{
        GameObject showerParticle;

        if (collisionObject.gameObject.tag == "Blue")
		{
            // material
            SetBallTexture(ballTextureBlue);

            // trail
            ballTrail.material = trailMaterialBlue;

            // particle
            showerParticle = Instantiate(GameHandler.Instance.particleShowerBlue, collisionObject.contacts[0].point, GameHandler.Instance.particleShowerBlue.transform.rotation) as GameObject;

            Destroy(showerParticle, 3.0f);

            // play paddle impact sound
            //soundHandler.paddleImpactSound.PlayOneShot(soundHandler.paddleImpactSound.clip);
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.paddleImpactSound.clip);
        }
		else if(collisionObject.gameObject.tag == "Orange")
		{
            // material
            SetBallTexture(ballTextureOrange);

            // trail
            ballTrail.material = trailMaterialOrange;

            // particle
            showerParticle = Instantiate(GameHandler.Instance.particleShowerOrange, collisionObject.contacts[0].point, GameHandler.Instance.particleShowerWhite.transform.rotation) as GameObject;

            Destroy(showerParticle, 3.0f);

            // play paddle impact sound
            //soundHandler.paddleImpactSound.PlayOneShot(soundHandler.paddleImpactSound.clip);
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.paddleImpactSound.clip);
        }
		else if(collisionObject.gameObject.tag == "WhitePillar")
		{
            // material
            SetBallTexture(ballTextureWhiteEmission);

            // trail
            ballTrail.material = trailMaterialWhite;

            // particle
            showerParticle = Instantiate(GameHandler.Instance.particleShowerWhite, collisionObject.contacts[0].point, GameHandler.Instance.particleShowerWhite.transform.rotation) as GameObject;

            Destroy(showerParticle, 3.0f);

            // play wall impact sound
            //soundHandler.wallImpactSound.PlayOneShot(soundHandler.wallImpactSound.clip);
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.wallImpactSound.clip);
        }
		else
		{
			// material
			SetBallTexture(ballTextureWhiteEmission);

			// trail
			ballTrail.material = trailMaterialWhite;

            // play wall impact sound
            //SoundHandler.Instance.PlayAudio(SoundHandler.Instance.wallImpactSound.clip);soundHandler.wallImpactSound.PlayOneShot(soundHandler.wallImpactSound.clip);
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.wallImpactSound.clip);

            // particle
            showerParticle = Instantiate(GameHandler.Instance.particleShowerWhite, collisionObject.contacts[0].point, GameHandler.Instance.particleShowerWhite.transform.rotation) as GameObject;
			//GameObject shockwaveParticle = Instantiate(GameHandler.Instance.particleShockwave, collisionObject.contacts[0].point, GameHandler.Instance.particleShockwave.transform.rotation) as GameObject;
			GameObject shockwaveParticle = Instantiate(GameHandler.Instance.particleShockwave, collisionObject.contacts[0].point, collisionObject.transform.rotation) as GameObject;	// placeholder

			// set particle shockwave rotation
			//shockwaveParticle.GetComponent<ParticleSystem>().startRotation3D = new Vector3(0.0f, collisionObject.transform.rotation.y * Mathf.Deg2Rad, 0.0f);

			// destroy particles
			Destroy(showerParticle, 2.0f);
			Destroy(shockwaveParticle, 5.5f);
		}

        // play camera animation
        cameraAnimator.SetTrigger("collision");

        // increase ball speed
        switch (GameHandler.Instance.ballTypes)
        {
            case GameHandler.BallTypes.Standard:
                break;
            case GameHandler.BallTypes.Speedster:
                ballSpeed += ballSpeedIncrease;
                break;
        }

		// screenshake
		StartCoroutine(NormalScreenShake());
	}

	// goal happened
	private void OnTriggerExit(Collider triggerObject)
	{
        GameObject explosionParticleBlue;
        GameObject explosionParticleOrange;

        if (triggerObject.gameObject.tag == "BlueGoal" && gameObject.transform.position.x <= -inGoalPosX)
		{
            // increase orange points
            GameHandler.Instance.pointsOrange++;

            uiHandler.PointsIncreaseOrange();

            // instantiate particle explosion
            explosionParticleBlue = Instantiate(GameHandler.Instance.particleExplosionBlue, triggerObject.gameObject.transform.position, GameHandler.Instance.particleExplosionBlue.transform.rotation) as GameObject;

            // destroy particle explosion
            Destroy(explosionParticleBlue, 3.0f);
        }
		else if(triggerObject.gameObject.tag == "OrangeGoal" && gameObject.transform.position.x >= inGoalPosX)
		{
            // increase blue points
			GameHandler.Instance.pointsBlue++;

            uiHandler.PointsIncreaseBlue();

            // instantiate particle explosion
            explosionParticleOrange = Instantiate(GameHandler.Instance.particleExplosionOrange, triggerObject.gameObject.transform.position, GameHandler.Instance.particleExplosionBlue.transform.rotation) as GameObject;

            // destroy particle explosion
            Destroy(explosionParticleOrange, 3.0f);
        }

        // play camera animation
        cameraAnimator.SetTrigger("goalScored");

        // play goal impact sound
        //soundHandler.goalImpactSound.PlayOneShot(soundHandler.goalImpactSound.clip);
        SoundHandler.Instance.PlayAudio(SoundHandler.Instance.goalImpactSound.clip);

        // respawn ball
        ResetBall();

		// screenshake
		StartCoroutine(GoalScreenShake());
	}

	// kick off ball functionality
	private IEnumerator KickOff()
	{
		ballSpeedZ = Random.Range (-randomBallSpeedZ, randomBallSpeedZ);
		lORr = Random.Range (-1.0f, 1.0f);

        GameHandler.Instance.ChangeStateTo(GameHandler.GameStates.Playing);

        yield return new WaitForSeconds(GameHandler.Instance.kickOffDelay);

        // shoot ball in random direction
        if (lORr >= 0.5f)
		{
            // shoot right
            //ballRigidbody.velocity = new Vector3(ballSpeed, 0.0f, ballSpeedZ);
            ballRigidbody.AddForce(ballSpeed, 0.0f, ballSpeedZ, ForceMode.Impulse);

            // play kickoff sound
            //soundHandler.kickOffSound.PlayOneShot(soundHandler.kickOffSound.clip);
            //soundHandler.playerGoSound.PlayOneShot(soundHandler.playerGoSound.clip);
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.playerGoSound.clip);
        }
		else
		{
            // shoot left
            //ballRigidbody.velocity = new Vector3(ballSpeed * -1.0f, 0.0f, ballSpeedZ);
            ballRigidbody.AddForce(ballSpeed * -1.0f, 0.0f, ballSpeedZ, ForceMode.Impulse);

            // play kickoff sound
            //soundHandler.kickOffSound.PlayOneShot(soundHandler.kickOffSound.clip);
            //soundHandler.playerGoSound.PlayOneShot(soundHandler.playerGoSound.clip);
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.playerGoSound.clip);
        }
    }

	private void ResetBall()
	{
        GameHandler.Instance.isBlueReady = false;
        GameHandler.Instance.isOrangeReady = false;

        GameHandler.Instance.ChangeStateTo(GameHandler.GameStates.WaitingForPlayer);

        // velocity
        ballRigidbody.velocity = Vector3.zero;

		// position
		gameObject.transform.position = new Vector3(0.0f, 2.5f, 0.0f);

		// material
		SetBallTexture(ballTextureWhite);

		// trail
		ballTrail.material = trailMaterialWhite;

        switch (GameHandler.Instance.ballTypes)
        {
            case GameHandler.BallTypes.Speedster:
                ballSpeed = ballSpeedDefault;
                break;
        }
	}

	private IEnumerator NormalScreenShake()
	{
		// stop screenshake
		iTween.Stop();

		// rest camera position
		GameHandler.Instance.mainCamera.transform.position = GameHandler.Instance.mainCameraPos;

		yield return new WaitForSeconds (0.01f);

		// screenshake
		screenShake.NormalScreenShakeCamera(GameHandler.Instance.mainCamera);
	}

	private IEnumerator GoalScreenShake()
	{
		// stop screenshake
		iTween.Stop();

		// rest camera position
		GameHandler.Instance.mainCamera.transform.position = GameHandler.Instance.mainCameraPos;

		yield return new WaitForSeconds (0.01f);

		// screenshake
		screenShake.GoalScreenShakeCamera(GameHandler.Instance.mainCamera);
	}

	private void SetBallTexture(Texture ballTexture)
	{
		// material
		ballMaterial.material.SetTexture("_EmissionMap", ballTexture);
	}
}
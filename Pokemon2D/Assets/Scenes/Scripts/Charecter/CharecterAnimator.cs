using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;


    [SerializeField] List<Sprite> torchDownSprites;
    [SerializeField] List<Sprite> torchUpSprites;
    [SerializeField] List<Sprite> torchRightSprites;
    [SerializeField] List<Sprite> torchLeftSprites;

    [SerializeField] List<Sprite> surfSprites;
    [SerializeField] FacingDirection defaultDir = FacingDirection.Down;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool IsJumping { get; set; }

    public bool IsSurfing { get; set; }

    public bool IsSHoldingTorch{ get; set; }





    // States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator torchDownAnim;
    SpriteAnimator torchUpAnim;
    SpriteAnimator torchRightAnim;
    SpriteAnimator torchLeftAnim;

    SpriteAnimator currentAnim;
    bool wasPreviouslyMoving;

    // Refrences
    SpriteRenderer spriteRenderer;

    private void Start()
    {
  
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        torchDownAnim = new SpriteAnimator(torchDownSprites, spriteRenderer);
        torchUpAnim = new SpriteAnimator(torchUpSprites, spriteRenderer);
        torchRightAnim = new SpriteAnimator(torchRightSprites, spriteRenderer);
        torchLeftAnim = new SpriteAnimator(torchLeftSprites, spriteRenderer);


        SetFacingDir(defaultDir);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        
        var prevAnim = currentAnim;

        if(!IsSurfing)
        {
            if (MoveX == 1)
                currentAnim = walkRightAnim;
            else if (MoveX == -1)
                currentAnim = walkLeftAnim;
            else if (MoveY == 1)
                currentAnim = walkUpAnim;
            else if (MoveY == -1)
                currentAnim = walkDownAnim;

            if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
                currentAnim.Start();

            if (IsMoving)
                currentAnim.HandleUpdate();
            else
                spriteRenderer.sprite = currentAnim.Frames[0];
        } else
        {
            if (MoveX == 1)
                spriteRenderer.sprite = surfSprites[2];
            else if (MoveX == -1)
                spriteRenderer.sprite = surfSprites[3];
            else if (MoveY == 1)
                spriteRenderer.sprite = surfSprites[1];
            else if (MoveY == -1)
                spriteRenderer.sprite = surfSprites[0];
        }

        if(IsSHoldingTorch)
        if (MoveX == 1)
            currentAnim = torchRightAnim;
        else if (MoveX == -1)
            currentAnim = torchLeftAnim;
        else if (MoveY == 1)
            currentAnim = torchUpAnim;
        else if (MoveY == -1)
            currentAnim = torchDownAnim;




        wasPreviouslyMoving = IsMoving;
    }

    public void SetFacingDir(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
            MoveX = 1f;
        else if (dir == FacingDirection.Left)
            MoveX = -1f;
        else if (dir == FacingDirection.Down)
            MoveY = -1f;
        else if (dir == FacingDirection.Up)
            MoveY = 1f;
    }
    public FacingDirection DefultDirection 
    {
        get => defaultDir;
    }
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}

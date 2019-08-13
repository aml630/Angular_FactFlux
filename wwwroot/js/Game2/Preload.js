function preload() {
    // Preload images
    this.load.image(
        "sky",
        "https://raw.githubusercontent.com/cattsmall/Phaser-game/5-2014-game/assets/sky.png"
    );
    this.load.image(
        "ground",
        "https://phaser.io/content/tutorials/making-your-first-phaser-3-game/platform.png"
    );
    this.load.image(
        "bomb",
        "assets/pinkBomb.jpg"
    );
    this.load.spritesheet(
        "dude",
        "https://raw.githubusercontent.com/cattsmall/Phaser-game/5-2014-game/assets/dude.png",
        {
            frameWidth: 32,
            frameHeight: 48
        }
    );
    this.load.spritesheet(
        "baddie",
        "https://raw.githubusercontent.com/cattsmall/Phaser-game/5-2014-game/assets/baddie.png",
        {
            frameWidth: 32,
            frameHeight: 32
        }
    );
    this.load.spritesheet(
        "star",
        "https://raw.githubusercontent.com/cattsmall/Phaser-game/5-2014-game/assets/baddie.png",
        {
            frameWidth: 32,
            frameHeight: 32
        }
    );
}
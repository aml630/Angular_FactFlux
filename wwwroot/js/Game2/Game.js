var config = {
  type: Phaser.AUTO,
  width:  800,
  height: 600,
  parent: 'parent',
  pixelArt: true,
  physics: {
    default: "arcade",
    arcade: {
      gravity: { y: 300 },
      debug: false
    }
  },
  scene: {
    preload: preload,
    create: create,
    update: update
  }
};

var game = new Phaser.Game(config);






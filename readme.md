### Gobang

This is a Gobang game that I developed in my sophomore year. Its innovation is that it adopts the learning method of neural network.I used the paper of AlphaGo for reference to realize the creation and training of neural network. Its training is based on tensorflow with GPU and takes Keras as the front-end framework.

I make a [video](https://www.bilibili.com/video/av70540487) on bilibili to show you how to run in detail.

#### Requirements

The game is allowed to run only on computers with. Net frame installed, but if you want to fight with AI of deep learning, you need the following environment:

- tensorflow >= 1.3.0
- keras >= 2.1.2
- cuda 10.0 (and cudnn)
- numpy

#### Play

Run **gobang.exe** to enter the game. 

**Config.txt** in the directory is the parameter configuration file. Players can choose **Human**, **MCTS AI** or **DeepMind**.

For example:

```
You:Human
Enemy:DeepMind
Serial Number:0000000001
```

![](http://106.15.93.194/assets/gobang.png)

#### Train

In order to get a large number of matches first, you can set both opponents to MCTS AI, so that after each match, the corresponding chess results will be saved in the **assets\train** folder, and you can rely on these to train the neural network.

When you want to train the network, first make sure you have a lot of data, and then run the **train.py** file in the directory.

**WARN**: Before you train the model, change the **rootdir** to the corresponding directory in **train.py**.

#### Update

- Fixed the problem that the chess data could not be saved due to the wrong path.

#### Agreement

This class library follows the open source protocol and is not allowed to be used commercially.

For more technical support, please contact me at mywineforever@outlook.com
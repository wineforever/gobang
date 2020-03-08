import numpy as np
from keras.models import Sequential, load_model
from keras.layers import Dense, Activation, Convolution2D,Flatten
from keras.optimizers import Adam,SGD
from keras.callbacks import ModelCheckpoint
from keras.regularizers import l2
import h5py

def create_model():
    lr = 1e-4
    policy_net = Sequential()
    policy_net.add(Convolution2D(filters=32,kernel_size=(5,5),padding='same', data_format="channels_first", activation="relu",kernel_regularizer=l2(lr),input_shape=(4,19,19)))
    policy_net.add(Convolution2D(filters=64,kernel_size=(3,3),padding='same', data_format="channels_first", activation="relu",kernel_regularizer=l2(lr)))
    policy_net.add(Convolution2D(filters=128,kernel_size=(3,3),padding='same', data_format="channels_first", activation="relu",kernel_regularizer=l2(lr)))
    policy_net.add(Convolution2D(filters=4,kernel_size=(1,1),padding='same', data_format="channels_first", activation="relu",kernel_regularizer=l2(lr)))
    policy_net.add(Flatten())
    policy_net.add(Dense(361,activation="softmax",kernel_regularizer=l2(lr)))
    adam = Adam(lr=2e-4)
    policy_net.compile(optimizer=adam,loss='categorical_crossentropy')
    policy_net.save('policy_model.h5')

def policy_train(state_input,mcts_probs,model):
    state_input_union = np.array(state_input).reshape(-1,4,19,19)
    mcts_probs_union = np.array(mcts_probs).reshape(-1,361)
    loss = model.evaluate(state_input_union,mcts_probs_union,batch_size=512,verbose=1)
    model.fit(state_input_union,mcts_probs_union,batch_size=512,epochs=5,verbose=1)
    model.save('policy_model.h5')
    return loss

from load_data import save_to_sheet
def policy_test(state_input,model):
    state_input = np.array(state_input).reshape(-1,4,19,19)
    results = model.predict(state_input)
    data = {}
    data['Policy'] = []
    for i in range(len(results[0])):
        data['Policy'].append((str)(results[0][i]))
    save_to_sheet(data,'policy.wf')
    return results[0]

from load_data import load_from_sheet
import os.path
def get_input(filepath):
    info = load_from_sheet(filepath)
    state_input = []
    policy_prob = []
    winner = []
    for i in range(int(len(info.items())/5)):
        #读取数据
        state_1 = info['Turn ' + str(i) + ' Current Self']
        state_2 = info['Turn ' + str(i) + ' Current Enemy']
        state_3 = info['Turn ' + str(i) + ' Enemy Recent Move']
        state_4 = info['Turn ' + str(i) + ' Player']
        #将String格式数据转换成Int
        state_1 =  [int(x) for x in state_1]
        state_2 =  [int(x) for x in state_2]
        state_3 =  [int(x) for x in state_3]
        state_4 =  [int(x) for x in state_4]
        #将List转换成19x19的二维数组
        state_1 = np.array(state_1).reshape(19,19)
        state_2 = np.array(state_2).reshape(19,19)
        state_3 = np.array(state_3).reshape(19,19)
        state_4 = np.array(state_4).reshape(19,19)
        #将上述List添加到state中，并变形为4x19x19的三维数组
        state = []
        state.append(state_1)
        state.append(state_2)
        state.append(state_3)
        state.append(state_4)
        state = np.array(state).reshape(4,19,19)
        #将state添加到state_input中
        state_input.append(state)
        #读取MCTS概率
        policy = info['Turn ' + str(i) + ' MCTS Prob']
        policy = [float(x) for x in policy]
        policy_prob.append(np.array(policy).reshape(19,19))
        winner.append(int(os.path.basename(filepath)[11:12]))
    return state_input, policy_prob, winner
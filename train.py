import numpy as np

inputs = []
policys = []
winners = []

from net import get_input
import os
import os.path
from Wineforever_PyBar import Bar

batch_size = 20

from net import create_model
#create_model()

rootdir = 'E:\\gobang\\train'
list = os.listdir(rootdir)

bar = Bar('import progress',batch_size)

for T in range(0,50):
    inputs = []
    policys = []
    winners = []
    for i in range(T*batch_size,T*batch_size+batch_size):
        path = os.path.join(rootdir,list[i])
        if os.path.isfile(path):
            input,policy,winner = get_input(path)
            inputs.extend(input)
            policys.extend(policy)
            winners.extend(winner)
        bar.next()

    from keras.models import load_model
    policy_model = load_model('policy_model.h5')

    from net import policy_train
    policy_loss = policy_train(inputs,policys,policy_model)

    print('interation:')
    print(T)


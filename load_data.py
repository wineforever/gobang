def load(filepath):
    f = open(filepath,encoding='utf-8')
    data = f.read()
    return data

def save(data,filepath):
    with open(filepath,'w', encoding='utf-8') as f:
        f.write(data)

def load_from_sheet(filepath):
    data = load(filepath)
    FLAG = [False,False]
    res = ['','']
    Data = {}
    for i in range(len(data)-1):
        if data[i-1]=='［':
            FLAG[0] = True
            res[0] = ''
        elif data[i-1] == '　' and data[i] != '　' and data[i] != '［':
            FLAG[1] = True
            res[1] = ''
        if FLAG[0]:
            res[0] += data[i]
        elif FLAG[1]:
            res[1] += data[i]
        if data[i+1] == '］':
            FLAG[0] = False
            Data[res[0]] = []
        elif data[i+1] == '　' and FLAG[1] and data[i] != '］':
            FLAG[1] = False
            Data[res[0]].append(res[1])
    return Data

def save_to_sheet(data,filepath):
    Data = []
    for key, value in data.items():
        Data.append('［' + key + '］')
        for i in range(len(value)):
            if value[i] == '':
                Data.append('　' + ' ' + '　')
            else:
                Data.append('　' + value[i] + '　')
    Data = ''.join(Data)
    save(Data,filepath)
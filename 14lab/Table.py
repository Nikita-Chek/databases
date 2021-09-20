from tkinter import *
from tkinter import ttk
from DAL import MongoCRUD

class Table:
    def __init__(self, root, data):
        total_rows = len(data)
        total_columns = 4
        self.e=[]
        self.e.append( Label(root, width=10, text='Name', borderwidth=2, fg='white',
                  font=('Arial', 18, 'bold'), bg='red'))
        self.e[0].grid(row=0, column=0+2)
        self.e.append(Label(root, width=10, text='Age', borderwidth=2, fg='white',
                  font=('Arial', 18, 'bold'), bg='red'))
        self.e[1].grid(row=0, column=1+2)
        self.e.append(Label(root, width=10, text='Exams', borderwidth=2, fg='white',
                  font=('Arial', 18, 'bold'), bg='red'))
        self.e[2].grid(row=0, column=2+2)
        self.e.append(Label(root, width=10, text='Group', borderwidth=2, fg='white',
                  font=('Arial', 18, 'bold'), bg='red'))
        self.e[3].grid(row=0, column=3+2)
        for i in range(1, total_rows + 1):
            for j in range(total_columns):
                if j == 3:
                    self.e = ttk.Combobox(root,
                                     values=[
                                         data[i - 1][j].get('Name'),
                                         data[i - 1][j].get('Number')],
                                     font=('Arial', 18, 'bold'),
                                     foreground='red')
                    self.e.grid(row=i, column=j+2)
                    self.e['state'] = 'readonly'
                    self.e.current(0)
                elif j == 2:
                    exams = data[i - 1][j]
                    exams = [str(x.get('Name')) + ' - ' + str(x.get('Mark')) for x in exams]
                    self.e = ttk.Combobox(root, values=exams,
                                     font=('Arial', 18, 'bold'), foreground='red')
                    self.e.grid(row=i, column=j+2)
                    self.e['state'] = 'readonly'
                    self.e.current(0)
                else:
                    self.e = Entry(root, width=20, fg='Red', font=('Arial', 18, 'bold'))
                    self.e.grid(row=i, column=j+2)
                    self.e.insert(END, data[i - 1][j])
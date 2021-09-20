from tkinter import *
from tkinter import ttk
from DAL import MongoCRUD
from Table import Table
from functools import partial


def insert_window():
    window = Tk()
    # name
    label1 = Label(window, text='Name',
                   font=('Arial', 18, 'bold'), fg='red')
    label1.grid(row=0, column=0)
    entry1 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry1.grid(row=0, column=1)
    # age
    label2 = Label(window, text=' Age ',
                   font=('Arial', 18, 'bold'), fg='red')
    label2.grid(row=1, column=0)
    entry2 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry2.grid(row=1, column=1)
    # exams
    label3 = Label(window, text='Exams',
                   font=('Arial', 18, 'bold'), fg='red')
    label3.grid(row=2, column=0)

    label4 = Label(window, text='Math',
                   font=('Arial', 18, 'bold'), fg='red')
    label4.grid(row=3, column=0)
    entry4 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry4.grid(row=3, column=1)

    label5 = Label(window, text='Prog',
                   font=('Arial', 18, 'bold'), fg='red')
    label5.grid(row=4, column=0)
    entry5 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry5.grid(row=4, column=1)

    label6 = Label(window, text=' DB  ',
                   font=('Arial', 18, 'bold'), fg='red')
    label6.grid(row=5, column=0)
    entry6 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry6.grid(row=5, column=1)
    # group
    label7 = Label(window, text='Group Name',
                   font=('Arial', 18, 'bold'), fg='red')
    label7.grid(row=6, column=0)
    entry7 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry7.grid(row=6, column=1)

    label8 = Label(window, text='Group Number',
                   font=('Arial', 18, 'bold'), fg='red')
    label8.grid(row=7, column=0)
    entry8 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry8.grid(row=7, column=1)

    # button
    def insert_command():
        global data_dict
        d = {
            "Name": entry1.get(),
            "Age": int(entry2.get()),
            "Group": {
                "Name": entry7.get(),
                "Number": int(entry8.get())
            },
            "Exams": [
                {"Name": "Math", "Mark": int(entry4.get())},
                {"Name": "Prog", "Mark": int(entry5.get())},
                {"Name": "DB", "Mark": int(entry6.get())}]
        }
        c = MongoCRUD()
        c.add_student(d)
        data_dict = c.get_student()
        c.close()
        
        data = []
        for d in data_dict:
            data.append([d.get('Name'), d.get('Age'),
                         d.get('Exams'), d.get('Group')])
        
        button_delete = []
        for i in range(len(data)):
            button_delete.append(
                Button(frame, text="DELETE",
                       command=partial(delete_command, i)))
            button_delete[i].grid(row=i + 1, column=0)
        
        button_update = []
        for i in range(len(data)):
            button_update.append(
                Button(frame, text="UPDATE",
                    command=partial(update_window, i)))
            button_update[i].grid(row=i + 1, column=1)

        t = Table(frame, data)

    button_insert = Button(window, text="INSERT", font=('Arial', 18, 'bold'),
                    bg='red', fg='white', command=insert_command)
    button_insert.grid(row=9, column=1)


def delete_command(i):
    global data_dict,t,root,button_delete,button_update,frame
    c = MongoCRUD()
    c.remove_student({"_id": data_dict[i].get('_id')})
    data_dict = c.get_student()
    c.close()
    for child in frame.winfo_children():
        child.destroy()
    data = []
    
    for d in data_dict:
        data.append([d.get('Name'), d.get('Age'),
        d.get('Exams'), d.get('Group')])
        print(d)
    
    button_delete = []
    for i in range(len(data)):
        button_delete.append(
            Button(frame, text="DELETE",
                       command=partial(delete_command, i))
            )
        button_delete[i].grid(row=i + 1, column=0)
    
    button_update = []
    for i in range(len(data)):
        button_update.append(
            Button(frame, text="UPDATE",
                command=partial(update_window, i)))
        button_update[i].grid(row=i + 1, column=1)

    t = Table(frame, data)

def update_window(i):
    global data_dict,t,button_delete,button_update,frame
    dict_update = data_dict[i]
    window = Tk()
    # name
    label1 = Label(window, text='Name',
                   font=('Arial', 18, 'bold'), fg='red')
    label1.grid(row=0, column=0)
    entry1 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry1.insert(END, dict_update.get('Name'))
    entry1.grid(row=0, column=1)
    # age
    label2 = Label(window, text=' Age ',
                   font=('Arial', 18, 'bold'), fg='red')
    label2.grid(row=1, column=0)
    entry2 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry2.insert(END, dict_update.get('Age'))
    entry2.grid(row=1, column=1)
    # exams
    label3 = Label(window, text='Exams',
                   font=('Arial', 18, 'bold'), fg='red')
    label3.grid(row=2, column=0)

    label4 = Label(window, text='Math',
                   font=('Arial', 18, 'bold'), fg='red')
    label4.grid(row=3, column=0)
    entry4 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry4.insert(END, dict_update.get('Exams')[0].get('Mark'))
    entry4.grid(row=3, column=1)

    label5 = Label(window, text='Prog',
                   font=('Arial', 18, 'bold'), fg='red')
    label5.grid(row=4, column=0)
    entry5 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry5.insert(END, dict_update.get('Exams')[1].get('Mark'))
    entry5.grid(row=4, column=1)

    label6 = Label(window, text=' DB  ',
                   font=('Arial', 18, 'bold'), fg='red')
    label6.grid(row=5, column=0)
    entry6 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry6.insert(END, dict_update.get('Exams')[2].get('Mark'))
    entry6.grid(row=5, column=1)
    # group
    label7 = Label(window, text='Group Name',
                   font=('Arial', 18, 'bold'), fg='red')
    label7.grid(row=6, column=0)
    entry7 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry7.insert(END, dict_update.get('Group').get('Name'))
    entry7.grid(row=6, column=1)

    label8 = Label(window, text='Group Number',
                   font=('Arial', 18, 'bold'), fg='red')
    label8.grid(row=7, column=0)
    entry8 = Entry(window, font=('Arial', 18, 'bold'), fg='red')
    entry8.insert(END, dict_update.get('Group').get('Number'))
    entry8.grid(row=7, column=1)

    # button
    def update_command():
        global data_dict, frame, button_delete, button_update
        query = {"_id": dict_update.get('_id')}
        newvalues = { "$set": {
            "Name": entry1.get(),
            "Age": int(entry2.get()),
            "Group": {
                "Name": entry7.get(),
                "Number": int(entry8.get())
            },
            "Exams": [
                {"Name": "Math", "Mark": int(entry4.get())},
                {"Name": "Prog", "Mark": int(entry5.get())},
                {"Name": "DB", "Mark": int(entry6.get())}]
        }}
        c = MongoCRUD()
        c.update_student(query, newvalues)
        data_dict = c.get_student()
        c.close()
        
        data = []
        for d in data_dict:
            data.append([d.get('Name'), d.get('Age'),
                         d.get('Exams'), d.get('Group')])
        
        button_delete = []
        for i in range(len(data)):
            button_delete.append(
                Button(frame, text="DELETE",
                       command=partial(delete_command, i)))
            button_delete[i].grid(row=i + 1, column=0)
        button_update = []
        for i in range(len(data)):
            button_update.append(
                Button(frame, text="UPDATE",
                    command=partial(update_window, i)))
            button_update[i].grid(row=i + 1, column=1)

        t = Table(frame, data)

    button_up = Button(window, text="UPDATE", font=('Arial', 18, 'bold'),
                    bg='red', fg='white', command=update_command)
    button_up.grid(row=9, column=1)






c = MongoCRUD()
data_dict = c.get_student()
c.export_studs_to_file('studs.json')
c.close()

data = []
for d in data_dict:
    data.append([d.get('Name'), d.get('Age'), d.get('Exams'), d.get('Group')])

root = Tk()
frame = Frame(root)
frame.grid()
t = Table(frame, data)
button_insert = Button(frame, text="INSERT ", font=('Arial', 14, 'bold'), fg='Red',
                 command=insert_window)
button_insert.grid(row=1, column=6)

# print(data_dict[i].get('Name'))

button_delete = []
for i in range(len(data)):
    button_delete.append(
        Button(frame, text="DELETE",
               command=partial(delete_command, i))
    )
    button_delete[i].grid(row=i + 1, column=0)

button_update = []
for i in range(len(data)):
    button_update.append(
        Button(frame, text="UPDATE",
               command=partial(update_window, i))
    )
    button_update[i].grid(row=i + 1, column=1)

root.mainloop()
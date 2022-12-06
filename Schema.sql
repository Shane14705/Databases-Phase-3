/*Initialization*/
CREATE SCHEMA PHASE3;
USE PHASE3;

/* Notes:
   For LOCATION datatype, we are using MySQL's POINT datatype that holds an x and y coordinate (lat/long) 
   For things such as names, we are assuming a max length of 32 Chars
   For currency, we are using the GAAP standard of a decimal number with 13 precision and 4 places after the decimal
   For color, we are using 6 chars to store a HEX color value
   */

/* We are going to create the tables that don't rely on any other tables first!*/
CREATE TABLE CUSTOMER (
    Customer_ID INT NOT NULL ,
    Delivery_Location POINT ,
    First_Name VARCHAR(32) NOT NULL ,
    Last_Name VARCHAR(32) NOT NULL ,
    Birth_Date DATE NOT NULL ,
    Phone_Number CHAR(10) NOT NULL ,
    Age INT NOT NULL ,
    PRIMARY KEY (Customer_ID)
);

CREATE TABLE ITEM (
    Item_ID INT NOT NULL ,
    Age_Requirement INT ,
    PRICE DECIMAL(13, 4) NOT NULL ,
    Quantity_Available INT NOT NULL ,
    Department_Number INT NOT NULL ,
    Aisle INT NOT NULL ,
    Shelf_Location INT NOT NULL ,
    PRIMARY KEY (Item_ID)
);

CREATE TABLE COURIER (
    Courier_ID INT NOT NULL ,
    First_Name VARCHAR(32) NOT NULL ,
    Last_Name VARCHAR(32) NOT NULL ,
    Phone_Number CHAR(10) NOT NULL ,
    Current_Location POINT ,
    Available BOOL NOT NULL ,
    PRIMARY KEY (Courier_ID)
);

CREATE TABLE EMPLOYEE (
    Employee_ID INT NOT NULL ,
    Cumulative_Pickrate FLOAT NOT NULL ,
    Salary DECIMAL(13, 4) NOT NULL,
    First_Name VARCHAR(32) NOT NULL ,
    Last_Name VARCHAR(32) NOT NULL ,
    PRIMARY KEY (Employee_ID)
);

/* Now we can carefully begin on the more dependent tables, beginning with the ones that have the least references. */

CREATE TABLE REGISTERED_CARS (
    Courier_ID INT NOT NULL ,
    LicensePlate_Number VARCHAR(8) NOT NULL ,
    Model VARCHAR(32) ,
    Color CHAR(6) ,
    PRIMARY KEY (LicensePlate_Number),
    FOREIGN KEY (Courier_ID) REFERENCES COURIER(Courier_ID)
);

CREATE TABLE JOB_ROLES (
    Employee_ID INT NOT NULL ,
    Role_Name VARCHAR(32) NOT NULL ,
    Department_Number INT NOT NULL ,
    PRIMARY KEY (Employee_ID, Role_Name, Department_Number),
    FOREIGN KEY (Employee_ID) REFERENCES EMPLOYEE(Employee_ID)
);

CREATE TABLE PICK_WALK (
    Start_Timestamp DATETIME NOT NULL ,
    Employee_ID INT NOT NULL ,
    Total_Quantity_Picked INT NOT NULL DEFAULT 0,
    End_Timestamp DATETIME ,
    Walk_Duration INT ,
    Pick_Rate FLOAT ,
    PRIMARY KEY (Start_Timestamp, Employee_ID),
    FOREIGN KEY (Employee_ID) REFERENCES EMPLOYEE(Employee_ID)
);

/*NAMED ORDERS instead of ORDER due to the latter conflicting with an existing MySQL keyword*/
CREATE TABLE ORDERS (
    Order_ID INT NOT NULL ,
    Order_Total DECIMAL(13, 4) NOT NULL ,
    Order_Timestamp DATETIME NOT NULL ,
    Order_Status INT NOT NULL DEFAULT 0,
    Courier_ID INT ,
    Pickup_Time DATETIME ,
    Delivery_Time DATETIME ,
    Hours_Elapsed INT ,
    Distance_Remaining FLOAT ,
    Estimated_Delivery_Time DATETIME,
    Customer_ID INT NOT NULL ,
    PRIMARY KEY (Order_ID),
    FOREIGN KEY (Courier_ID) REFERENCES COURIER(Courier_ID),
    FOREIGN KEY (Customer_ID) REFERENCES CUSTOMER(Customer_ID)
);

CREATE TABLE PICK_LIST (
    Start_Timestamp DATETIME ,
    Employee_ID INT ,
    Item_ID INT NOT NULL ,
    Quantity_Needed INT NOT NULL ,
    Order_ID INT NOT NULL ,
    PRIMARY KEY (Item_ID, Order_ID),
    FOREIGN KEY (Start_Timestamp, Employee_ID) REFERENCES PICK_WALK(Start_Timestamp, Employee_ID),
    FOREIGN KEY (Item_ID) REFERENCES ITEM(Item_ID),
    FOREIGN KEY (Order_ID) REFERENCES ORDERS(Order_ID)
);

CREATE TABLE ITEMS_ORDERED (
    Order_ID INT NOT NULL ,
    Item_ID INT NOT NULL ,
    Quantity_Requested INT NOT NULL ,
    PRIMARY KEY (Order_ID, Item_ID),
    FOREIGN KEY (Order_ID) REFERENCES ORDERS(Order_ID),
    FOREIGN KEY (Item_ID) REFERENCES ITEM(Item_ID)
);

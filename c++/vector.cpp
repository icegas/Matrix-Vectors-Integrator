#include "vector.h"


Vector::Vector(int rows)
{
    this->Rows = rows;
    Data.resize(rows);
}

Vector::~Vector(){

}

double& Vector::operator [](int index){
    return Data[index];
}

int Vector::RowsCount(){
    return this->Rows;
}

Vector Vector::operator +(Vector &v1){
    Vector result(v1.RowsCount());
    if(v1.RowsCount() != this->RowsCount())
        QMessageBox::information(NULL,"Error","Rows of Vector must be equal");
    else
    for(int i=0; i < v1.RowsCount(); i++)
        result[i] = this->Data[i] + v1[i];
    return result;
}

Vector Vector::operator -(Vector &v1){
    Vector result(v1.RowsCount());
    if(v1.RowsCount() != this->RowsCount())
        QMessageBox::information(NULL,"Error","Rows of Vector must be equal");
    else
    for(int i = 0; i < v1.RowsCount(); i++)
        result[i] = this->Data[i] - v1[i];
    return result;
}

void Vector::ReSize(int rows){
    Rows = rows;
    Data.resize(rows);
}

double Vector::operator *(Vector &v1){
    double result;
    if(v1.RowsCount() != this->RowsCount())
        QMessageBox::information(NULL,"Error","Rows of Vector must be equal");
    else
    for(int i = 0; i < v1.RowsCount(); i++)
        result += this->Data[i] * v1[i];
    return result;
}

/*Vector Vector::operator *(Matrix &m1){
    Vector result(this->RowsCount());
    for(int i = 0; i < m1.RowsCount(); i++)
        for(int j = 0; j < m1.ColumnsCount(); j++)
            result[i] += this->Data[j] * m1[i][j];
    return result;
}*/


Vector Vector::operator *(double value){
    Vector result(this->RowsCount());
    for(int i = 0; i < this->RowsCount(); i++)
        result[i] = this->Data[i] * value;
    return result;
}

Vector Vector::operator /(double value){
    Vector result(this->RowsCount());
    for(int i = 0; i < this->RowsCount(); i++)
        result[i] = this->Data[i] / value;
    return result;
}

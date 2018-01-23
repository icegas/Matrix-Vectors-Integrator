#include "matrix.h"
//#include "vector.cpp"

//#include <QMessageBox>

Matrix::Matrix(int rows, int columns)
{
    this->Rows = rows;
    this->Columns = columns;
    Data.resize(rows);
    for(int i=0; i < rows; i++)
        Data[i].resize(columns);
}

double const* Matrix::operator[]( int row ) const
{
   return &Data[row][0];
}

double* Matrix::operator[]( int column )
{
    return &Data[column][0];
}

void Matrix::ReSize(int rows, int columns){
    this->Rows = rows;
    this->Columns = columns;
    Data.resize(rows);
    for(int i=0; i < rows; i++)
        Data[i].resize(columns);
}

int Matrix::ColumnsCount(){
    return this->Columns;
}

int Matrix::RowsCount(){
    return this->Rows;
}

Matrix Matrix::Flip(){
    Matrix result(this->ColumnsCount(), this->RowsCount());

    for(int i = 0; i < this->RowsCount(); i++)
        for(int j = 0; j < this->ColumnsCount(); j++)
            result[j][i] = this->Data[i][j];
    return result;
}

Matrix& Matrix::operator =(Matrix *m){
    for(int i = 0; i < this->RowsCount(); i++)
        for(int j = 0; j < this->ColumnsCount(); j++)
            this->Data[i][j] = m->Data[i][j];
    return *this;
}

Matrix& Matrix::operator /(double val){
    for(int i = 0; i < this->RowsCount(); i++)
        for(int j = 0; j < this->ColumnsCount(); j++)
            (*this)[i][j] /= val;
    return *this;
}

Matrix& Matrix::operator =(double val){
    for(int i = 0; i < this->RowsCount(); i++)
        for(int j = 0; j < this->ColumnsCount(); j++)
            (*this)[i][j] = val;
    return *this;
}

Matrix Matrix::operator *(Matrix &m){
    double temp;
    Matrix result(this->RowsCount(), m.ColumnsCount());

    for(int i = 0; i < m.ColumnsCount(); i++)
        for(int j = 0; j < this->RowsCount(); j++){
            temp = 0;
            for(int k = 0; k < this->ColumnsCount(); k++)
                temp += this->Data[j][k] * m[k][i];
            result[j][i] = temp;

        }
    return result;
}

Vector Matrix::operator *(Vector &v){
    Vector result(v.RowsCount());
    for(int i = 0; i < this->RowsCount(); i++)
        for(int j = 0; j < this->ColumnsCount(); j++)
            result[i] += this->Data[i][j] * v[j];
    return result;
}

Matrix Matrix::operator +(Matrix &m){
    Matrix result(m.RowsCount(), m.ColumnsCount());

    for(int i = 0; i < m.RowsCount(); i++)
        for(int j = 0; j < m.ColumnsCount(); j++)
            result[i][j] = this->Data[i][j] + m[i][j];
    return result;
}

Matrix Matrix::MultDecRow(Matrix m,int drow, int row,double val){
     for (int i = 0; i < m.ColumnsCount(); i++)
         m[row][i] = m[row][i] - val * m[drow][i];
     return m;
}

Matrix Matrix::AddRow(Matrix m, int Arow, int row){
    for (int i = 0; i < m.ColumnsCount(); i++)
         m[row][i] = m[row][i] + m[Arow][i];
    return m;
}

Matrix Matrix::MultRow(Matrix m,int row,double val){
    for (int i = 0; i < m.ColumnsCount(); i++)
        m[row][i] = m[row][i] * val;

    return m;
}

Matrix Matrix::SwapRow(Matrix m, int Row, int SwapRow){
    double temp;

    for (int i = 0; i < m.ColumnsCount(); i++)
    {
        temp = m[Row][i];//temp = m.GetElement(i,Row);
        m[Row][i] = m[SwapRow][i];//m.SetElement(i, Row, m.GetElement(i, SwapRow));
        m[SwapRow][i] = temp;// m.SetElement(i, SwapRow, temp);
    }
    return m;
}

Matrix Matrix::Inverse(){
    Matrix m(Columns, Rows);
    Matrix E(Columns, Rows);
    Matrix Zero(Columns, Rows);
    int k;

    for (int i = 0; i < Rows; i++)
         for (int j = 0; j < Columns; j++)
              m[j][i] = (*this)[j][i];//.SetElement(i, j, Data[i, j]);

    for (int i = 0; i < Rows; i++)
        for (int j = 0; j < Columns; j++)
             if(i==j)
                E[i][j] = 1;

     for (int i = 0; i < Rows; i++)
          for (int j = 0; j < Columns; j++)
                Zero[j][i] = 0;


     //начало
     for (int j = 0; j < Columns; j++)
     {
         k = Rows - j;
         //проверка значения на 0 если 0 то меняем местами строки
         while (!m[j][j])
         {
               if (k != 1)
               {
                   m = m.SwapRow(m, j , Rows - k + 1);//m.SwapRow(ref m, j, Rows - k + 1);
                   E = E.SwapRow(E, j, Rows - k + 1);
                   k--;
                }
                else
                {
                   QMessageBox::information(NULL,"Error","Matrix nonsingular.Please Change Matrix Value and continue.");
                   return Zero;
                }


          }

              E = E.MultRow( E, j, 1 / m[j][j]);
              m = m.MultRow( m, j, 1 / m[j][j]);

              for (int i = j + 1; i < Rows; i++)
              {
                   E = E.MultDecRow( E, j, i, m[i][j]);
                   m = m.MultDecRow( m, j, i, m[i][j]);

              }

          }

          //обратный ход
          for (int i = Rows - 1; i > 0; i--)
          {
                for (int j = 0; j < i; j++)
                {
                    E = E.MultDecRow( E, i, j, m[j][i]);
                    m = m.MultDecRow( m, i, j, m[j][i]);
                }
          }


          return E;

}

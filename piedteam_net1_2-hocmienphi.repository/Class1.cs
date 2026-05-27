namespace piedteam_net1_2_hocmienphi.repository
{
    public class Class1
    {
    }
    // code first | database first
    // ORM
    // thong thuong để mà tren code co the lam viec dc voi db.
        // thi minh can phai mapping từ các table len code de co the de dang thao tac voi du lieu
        
    // database first:
    // minh lam viec voi 1 database da co san, (create Db bang SQL)
    // vo tao DB, setup cac field, cac mqh, rang buoc trong db
    // sau do tren code su dung cac Driver hoạc thu vien ORM để ket nôi xuong db
    // o tren code se tao cac class tuong ung voi cac table trong db
    // thang nay su dung khi minh da co sẵn db va đang dc su dung trong nhìu năm
        // dc Join vao du an Maintain
    
    //code first
    // minh se kh setup db thu công băng cac cau lenh query
        //create db, create constraint
    //minh se desgin cac db bang cac class trên code luon
        // tren code setup nhu the nao thi db se dc tao ra nhu the do
        // minh setup cac field, cac relationship
        // sau do minh anh xa. cac doan code do de tao ra table trong db
    //vay thi lam the nao de anh xa duoc từ code xuong cac table trong db
        // cau tra loi la: ORM - Object Relational Mapping
        // no se la thang trung gian đứng giua, lam nhìu thws
            // no se đọc các class tren code, doc cac attribute, doc cac cấu hình
                // sau đó tạo ra cac cau lệnh SQL để tạo cac bảng
            // no cung la thang ket hop voi LINQ
                // khi su dung cac ham Where,... Translate sang SQL
                // .WHERE() => SELECT * FROM TABLE WHERE ...
    // thang nay dc su dung khi
        //minh moi bat dau dư an, chưa có db nào cả
        // thiet ke bang code thì nó sẽ dễ dang Maintain hon (dễ nhìn,
        // dễ sửa đổi) hơn so voi cac cau lenh SQL
        
    // neu ma kh biet ve LINQ + EF thi coi nhu mat 99% suc manh
}


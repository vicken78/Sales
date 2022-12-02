﻿using Dapper;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using VMSales.Models;
// Example implementation of the rRepository

// Our model.

namespace VMSales.Logic
{

    public class DataBaseLayer : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #region Category
        public class CategoryRepository : Repository<CategoryModel> 
        {

            public CategoryRepository(IDatabaseProvider dbProvider) : base(dbProvider) { }

            public override async Task<bool> Insert(CategoryModel entity)
            {
                int newId = await Connection.QuerySingleAsync<int>("INSERT INTO category (category_name, description, creation_date) VALUES (@category_name, @description, @creation_date); SELECT last_insert_rowid()", new
                {
                    category_name = entity.category_name,
                    description = entity.description,
                    creation_date = System.DateTime.Now
                }, Transaction);

                entity.category_pk = newId;

                return true;
            }

            public override async Task<CategoryModel> Get(int id)
            {
                return await Connection.QuerySingleAsync<CategoryModel>("SELECT category_pk FROM category WHERE category_pk = @id", new { id }, Transaction);
            }

            public override async Task<IEnumerable<CategoryModel>> GetAll()
            {
                return await Connection.QueryAsync<CategoryModel>("SELECT category_pk, category_name, description, creation_date FROM category", null, Transaction);
            }

            public override async Task<bool> Update(CategoryModel entity)
            {
                // Could check for unset id, and throw exception.
                return (await Connection.ExecuteAsync("UPDATE category SET category_name = @category_name, description = @description WHERE category_pk = @id", new
                {
                    id = entity.category_pk,
                    category_name = entity.category_name,
                    description = entity.description

                }, Transaction)) == 1;
            }

            public override async Task<bool> Delete(CategoryModel entity)
            {
                // Could check for unset id, and throw exception.
                return (await Connection.ExecuteAsync("DELETE FROM category WHERE category_pk = @id", new { id = entity.category_pk }, Transaction)) == 1;
            }

            public override Task<IEnumerable<CategoryModel>> GetAllWithID(int id)
            {
                throw new System.NotImplementedException();
            }
        }
        #endregion

        #region Supplier
        public class SupplierRepository : Repository<SupplierModel>
        {

            public SupplierRepository(IDatabaseProvider dbProvider) : base(dbProvider) { }

            public override async Task<bool> Insert(SupplierModel entity)
            {
                int newId = await Connection.QuerySingleAsync<int>("INSERT INTO supplier (supplier_name, address, city, zip, state, country, phone, email) VALUES (@supplier_name, @address, @city, @zip, @state, @country, @phone, @email); SELECT last_insert_rowid()", new
                {
                    supplier_name = entity.supplier_name,
                    address = entity.address,
                    city = entity.city,
                    zip = entity.zip,
                    state = entity.state,
                    country = entity.country,
                    phone = entity.phone,
                    email = entity.email
                }, Transaction);

                entity.supplier_pk = newId;

                return true;
            }

            public override async Task<SupplierModel> Get(int id)
            {
                return await Connection.QuerySingleAsync<SupplierModel>("SELECT supplier_pk FROM supplier WHERE supplier_pk = @id", new { id }, Transaction);
            }

            public override async Task<IEnumerable<SupplierModel>> GetAll()
            {
                return await Connection.QueryAsync<SupplierModel>("SELECT supplier_pk, supplier_name, address, city, zip, state, country, phone, email FROM supplier", null, Transaction);
            }

            public override async Task<bool> Update(SupplierModel entity)
            {
                // Could check for unset id, and throw exception. // fix
                return (await Connection.ExecuteAsync("UPDATE supplier SET supplier_name = @supplier_name, address = @address, city = @city, zip = @zip, state = @state, country = @country, phone = @phone, email = @email WHERE supplier_pk = @id", new
                {
                    id = entity.supplier_pk,
                    supplier_name = entity.supplier_name,
                    address = entity.address,
                    city = entity.city,
                    zip = entity.zip,
                    state = entity.state,
                    country = entity.country,
                    phone = entity.phone,
                    email = entity.email
          
                }, Transaction)) == 1;
            }

            public override async Task<bool> Delete(SupplierModel entity)
            {
                // Could check for unset id, and throw exception.
                return (await Connection.ExecuteAsync("DELETE FROM supplier WHERE supplier_pk = @id", new { id = entity.supplier_pk }, Transaction)) == 1;
            }
            public override Task<IEnumerable<SupplierModel>> GetAllWithID(int id)
            {
                throw new System.NotImplementedException();
            }

        }
        #endregion
        #region PurchaseOrder
        //string sqlquery = "purchase_order, purchase_order_detail, supplier WHERE purchase_order.purchaseorder_pk = purchase_order_detail.purchaseorder_fk AND supplier.supplier_pk = purchase_order.supplier_fk AND supplier.supplier_pk='" + supplier_pk + "'";
        public class PurchaseOrderRepository : Repository<PurchaseOrderModel>
        {
            public PurchaseOrderRepository(IDatabaseProvider dbProvider) : base(dbProvider) { }

            public override async Task<bool> Insert(PurchaseOrderModel entity)
            {
                int newId = await Connection.QuerySingleAsync<int>("INSERT INTO category (category_name, description, creation_date) VALUES (@category_name, @description, @creation_date); SELECT last_insert_rowid()", new
                {
                    //category_name = entity.category_name,
                    //description = entity.description,
                    //creation_date = System.DateTime.Now
                }, Transaction);

                //entity.category_pk = newId;

                return true;
            }

            public override async Task<PurchaseOrderModel> Get(int id)
            {
                return await Connection.QuerySingleAsync<PurchaseOrderModel>("SELECT category_p FROM category WHERE category_pk = @id", new { id }, Transaction);
            }
  
            // get all purchase_order and purchase_order_detail
             public override async Task<IEnumerable<PurchaseOrderModel>> GetAll()
            {
                return await Connection.QueryAsync<PurchaseOrderModel>("SELECT " +
                    "distinct po.purchase_date, po.invoice_number, pod.lot_number, pod.lot_cost, pod.lot_qty," +
                    "pod.lot_name, pod.lot_description, pod.sales_tax, pod.shipping_cost " +
                    "FROM purchase_order as po, purchase_order_detail as pod, supplier as sup " +
                    "INNER JOIN purchase_order_detail on po.purchase_order_pk = pod.purchase_order_fk " +
                    "INNER JOIN supplier on sup.supplier_pk = po.supplier_fk;", null, Transaction);
            }
   
            // get all purchase_order and purchase_order_detail
            public override async Task<IEnumerable<PurchaseOrderModel>> GetAllWithID(int id)
            {
                return await Connection.QueryAsync<PurchaseOrderModel>("SELECT " +
                    "distinct po.purchase_date, po.invoice_number, pod.lot_number, pod.lot_cost, pod.lot_qty," +
                    "pod.lot_name, pod.lot_description, pod.sales_tax, pod.shipping_cost " +
                    "FROM purchase_order as po, purchase_order_detail as pod, supplier as sup " +
                    "INNER JOIN purchase_order_detail on po.purchase_order_pk = pod.purchase_order_fk " +
                    "INNER JOIN supplier on sup.supplier_pk = po.supplier_fk " +
                    "AND po.supplier_fk=@id", new { id }, Transaction);
            }
            
            public override async Task<bool> Update(PurchaseOrderModel entity)
            {
                // Could check for unset id, and throw exception.
                return (await Connection.ExecuteAsync("UPDATE category SET category_name = @category_name, description = @description WHERE category_pk = @id", new
                {
                    //id = entity.category_pk,
                    //category_name = entity.category_name,
                    //description = entity.description

                }, Transaction)) == 1;
            }

            public override async Task<bool> Delete(PurchaseOrderModel entity)
            {
                // Could check for unset id, and throw exception.
                return (await Connection.ExecuteAsync("DELETE FROM catego WHERE category_pk = @id", new { id = entity.supplier_fk }, Transaction)) == 1;
            }
        }

    }
            #endregion
}
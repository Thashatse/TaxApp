using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using TypeLibrary.Models;
using TypeLibrary.ViewModels;

namespace BLL
{
    public class DBHandler : IDBHandler
    {
        private IDBAccess db;

        public DBHandler()
        {
            db = new DBAccess();
        }

        #region Home Page Features
        public List<HomePageFeatures> GetHomePageFeatures()
        {
            return db.GetHomePageFeatures();
        }

        public bool UpdatedHomePageFeatures(Home_Page UpdateFeature)
        {
            return db.UpdatedHomePageFeatures(UpdateFeature);
        }
        #endregion

        #region Email/SMS Notifications
        public List<OGBkngNoti> GetOGBkngNotis()
        {
            return db.GetOGBkngNotis();
        }

        public bool updateNotiStatus(string bookingID, bool notiStatus)
        {
            return db.updateNotiStatus(bookingID, notiStatus);
        }
        #endregion

        #region Invoice/Sale
        public SALE getSale(string SaleID)
        {
            return db.getSale(SaleID);
        }

        public bool createProductSalesDTLRecord(SALES_DTL Sale)
        {
            return db.createProductSalesDTLRecord(Sale);
        }

        public bool removeProductSalesDTLRecord(SALES_DTL Sale)
        {
            return db.removeProductSalesDTLRecord(Sale);
        }

        public bool UpdateProductSalesDTLRecordQty(SALES_DTL Sale)
        {
            return db.UpdateProductSalesDTLRecordQty(Sale);
        }

        public bool createSalesRecord(SALE newSale)
        {
            return db.createSalesRecord(newSale);
        }
        #endregion

        #region User Accounts
        public bool updateStylistBio(EMPLOYEE bioUpdate)
        {
            return db.updateStylistBio(bioUpdate);
        }

        public bool updateUserAccountPassword(string password, string userID)
        {
            return db.updateUserAccountPassword(password, userID);
        }

        public USER GetAccountForRestCode(string code)
        {
            return db.GetAccountForRestCode(code);
        }

        public bool createRestCode(string emailOrUsername, string restCode)
        {
            return db.createRestCode(emailOrUsername, restCode);
        }

        public bool deactivateUser(string userID)
        {
            return db.deactivateUser(userID);
        }

        public USER checkForAccountTypeEmail(string identifier)
        {
            return db.checkForAccountTypeEmail(identifier);
        }
        #endregion

        #region Authentication
        public USER getPasHash(string identifier)
        {
            return db.getPasHash(identifier);
        }

        public USER logInEmail(string identifier, string password)
        {
            return db.logInEmail(identifier, password);
        }
        #endregion

        #region Products
        public PRODUCT CheckForProduct(string id)
        {
            return db.CheckForProduct(id);
        }
        
        public bool addAccessories(ACCESSORY a, PRODUCT p)
        {
            return db.addAccessories(a, p);
        }

        public bool addTreatments(TREATMENT t, PRODUCT p)
        {
            return db.addTreatments(t, p);
        }

        public SP_GetAllAccessories selectAccessory(string accessoryID)
        {
            return db.selectAccessory(accessoryID);
        }

        public SP_GetAllTreatments selectTreatment(string treatmentID)
        {
            return db.selectTreatment(treatmentID);
        }

        //addProduct
        public bool addProduct(PRODUCT addProduct)
        {
            return db.addProduct(addProduct);
        }

        public bool updateAccessories(ACCESSORY a, PRODUCT p)
        {
            return db.updateAccessories(a, p);
        }

        public bool updateTreatments(TREATMENT t, PRODUCT p)
        {
            return db.updateTreatments(t, p);
        }
        #endregion

        #region ProductTypes
        public bool addProductType(ProductType newType)
        {
            return db.addProductType(newType);
        }

        public bool editProductType(ProductType updateType)
        {
            return db.editProductType(updateType);
        }
        #endregion

        #region Product Orders
        public OrderViewModel getOrder(string orderID)
        {
            return db.getOrder(orderID);
        }

        public List<OrderViewModel> getOutStandingOrders()
        {
            return db.getOutStandingOrders();
        }

        public List<OrderViewModel> getPastOrders()
        {
            return db.getPastOrders();
        }

        public List<OrderViewModel> getProductOrderDL(string orderID)
        {
            return db.getProductOrderDL(orderID);
        }
        
        public bool newProductOrder(Order newOrder)
        {
            return db.newProductOrder(newOrder);
        }

        public bool newProductOrderDL(Order_DTL newOrderDL)
        {
            return db.newProductOrderDL(newOrderDL);
        }

        public Order CheckForOrder(string id)
        {
            return db.CheckForOrder(id);
        }
        #endregion

        #region Auto Product Orders
        public List<SP_GetAuto_Purchase_Products> getAutoPurchOrdProds()
        {
            return db.getAutoPurchOrdProds();
        }

        public bool newAutoPurchProd(Auto_Purchase_Products newProduct)
        {
            return db.newAutoPurchProd(newProduct);
        }

        public bool deleteAutoPurchProd(Auto_Purchase_Products product)
        {
            return db.deleteAutoPurchProd(product);
        }
        #endregion

        #region Stock Managment Settings
        public Stock_Management getStockSettings()
        {
            return db.getStockSettings();
        }

        public bool updateStockSettings(Stock_Management Update)
        {
            return db.updateStockSettings(Update);
        }
        #endregion

        #region Bookings
        public List<SP_GetBookingServices> getBookingServices(string bookingID)
        {
            return db.getBookingServices(bookingID);
        }

        public bool deleteBookingService(string BookingID, string ServiceID)
        {
            return db.deleteBookingService(BookingID, ServiceID);
        }

        public bool deleteSecondaryBooking(string BookingID)
        {
            return db.deleteSecondaryBooking(BookingID);
        }
        public SP_GetMultipleServicesTime getMultipleServicesTime(string primaryBookingID)
        {
            return db.getMultipleServicesTime(primaryBookingID);
        }
        #endregion

        #region Services

        public bool BLL_AddService(PRODUCT p, SERVICE s)
        {
            return db.AddService(p, s);
        }
        

        public List<SP_GetWidth> BLL_GetWidths()
        {
            return db.GetWidths();
        }

        public List<SP_GetLength> BLL_GetLengths()
        {
            return db.GetLengths();
        }

        public List<SP_GetStyles> BLL_GetStyles()
        {
            return db.GetStyles(); 
        }
        
        public bool BLL_AddBraidService(BRAID_SERVICE bs)
        {
            return db.AddBraidService(bs);
        }
       public SP_GetBraidService BLL_GetBraidServiceFromID(string serviceID)
        {
            return db.GetBraidServiceFromID(serviceID);
        }
        public SP_GetService BLL_GetServiceFromID(string serviceID)
        {
            return db.GetServiceFromID(serviceID);
        }
        #endregion

        #region Brands
        public List<SP_GetBrandsForProductType> getBrandsForProductType(char type)
        {
            return db.getBrandsForProductType(type);
        }

        public List<BRAND> getAllBrands()
        {
            return db.getAllBrands();
        }

        public BRAND getBrand(string BrandID)
        {
            return db.getBrand(BrandID);
        }

        public bool newBrand(BRAND newBrand)
        {
            return db.newBrand(newBrand);
        }

        public bool editBrand(BRAND brandUpdate)
        {
            return db.editBrand(brandUpdate);
        }

        public BRAND CheckForBrand(string id)
        {
            return db.CheckForBrand(id);
        }
        #endregion

        #region Supplier
        public List<Supplier> getSuppliers()
        {
            return db.getSuppliers();
        }

        public Supplier getSupplier(string suppID)
        {
            return db.getSupplier(suppID);
        }

        public bool newSupplier(Supplier newSupp)
        {
            return db.newSupplier(newSupp);
        }

        public bool editSupplier(Supplier suppUpdate)
        {
            return db.editSupplier(suppUpdate);
        }

        public Supplier CheckForSupplier(string id)
        {
            return db.CheckForSupplier(id);
        }
        #endregion

        #region Manager Dash Board
        public ManagerStats GetManagerStats()
        {
            return db.GetManagerStats();
        }
        #endregion

        #region search
        public Tuple<List<SP_ProductSearchByTerm>, List<SP_SearchStylistsBySearchTerm>> UniversalSearch(string searchTerm)
        {
            return db.UniversalSearch(searchTerm);
        }

        public List<SP_GetCustomerBooking> searchBookings(DateTime startDate, DateTime endDate)
        {
            return db.searchBookings(startDate, endDate);
        }
        #endregion

        #region Reviews
        public List<SP_GetReviews> getAllBookingReviews()
        {
            return db.getAllBookingReviews();
        }
        public bool reviewBooking(REVIEW r)
        {
            return db.reviewBooking(r);
        }
        public bool reviewStylist(REVIEW r)
        {
            return db.reviewStylist(r);
        }
        public bool updateBookingReview(REVIEW r)
        {
            return db.updateBookingReview(r);
        }
        public bool updateStylistReview(REVIEW r)
        {
            return db.updateStylistReview(r);
        }
        public REVIEW CheckForReview(string reviewID)
        {
            return db.CheckForReview(reviewID);
        }
        public List<SP_ReturnStylistNamesForReview> returnStylistNamesForReview(string customerID)
        {
            return db.returnStylistNamesForReview(customerID);
        }
        public REVIEW getStylistRating(string stylistID)
        {
            return db.getStylistRating(stylistID);
        }
        public REVIEW customersReviewForStylist(string customerID, string stylistID)
        {
            return db.customersReviewForStylist(customerID, stylistID);
        }
        public REVIEW customersReviewForBooking(string customerID, string bookingID)
        {
            return db.customersReviewForBooking(customerID, bookingID);
        }
        public List<SP_GetReviews> getCustomersReviews(string customerID)
        {
            return db.getCustomersReviews(customerID);
        }
        public List<SP_GetReviews> getReviewsOfStylist(string stylistID)
        {
            return db.getReviewsOfStylist(stylistID);
        }
        public List<SP_GetReviews> getAllStylistReviews()
        {
            return db.getAllStylistReviews();
        }
        public List<SP_GetCustomerBooking> getCustRecentBookings(string CustomerID)
        {
            return db.getCustRecentBookings(CustomerID);
        }
        #endregion

        #region Report
        #region Top Customer
        public List<productSalesReport> getCustomerSalesVolumeAll(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerSalesVolumeAll(startDate, endDate);
        }

        public List<productSalesReport> getCustomerSalesVolumeCredit(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerSalesVolumeCredit(startDate, endDate);
        }

        public List<productSalesReport> getCustomerSalesVolumeCash(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerSalesVolumeCash(startDate, endDate);
        }

        public List<productSalesReport> getCustomerSalesValueCredit(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerSalesValueCredit(startDate, endDate);
        }

        public List<productSalesReport> getCustomerSalesValueCash(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerSalesValueCash(startDate, endDate);
        }

        public List<productSalesReport> getCustomerSalesValueAll(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerSalesValueAll(startDate, endDate);
        }
        #endregion

        #region Top Customer Product
        public List<productSalesReport> getCustomerProductSalesVolumeAll(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerProductSalesVolumeAll(startDate, endDate);
        }

        public List<productSalesReport> getCustomerProductSalesVolumeCredit(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerProductSalesVolumeCredit(startDate, endDate);
        }

        public List<productSalesReport> getCustomerProductSalesVolumeCash(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerProductSalesVolumeCash(startDate, endDate);
        }

        public List<productSalesReport> getCustomerProductSalesValueCredit(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerProductSalesValueCredit(startDate, endDate);
        }

        public List<productSalesReport> getCustomerProductSalesValueCash(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerProductSalesValueCash(startDate, endDate);
        }

        public List<productSalesReport> getCustomerProductSalesValueAll(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerProductSalesValueAll(startDate, endDate);
        }
        #endregion

        #region Top Customer Service
        public List<productSalesReport> getCustomerServiceSalesVolumeAll(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerServiceSalesVolumeAll(startDate, endDate);
        }

        public List<productSalesReport> getCustomerServiceSalesVolumeCredit(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerServiceSalesVolumeCredit(startDate, endDate);
        }

        public List<productSalesReport> getCustomerServiceSalesVolumeCash(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerServiceSalesVolumeCash(startDate, endDate);
        }

        public List<productSalesReport> getCustomerServiceSalesValueCredit(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerServiceSalesValueCredit(startDate, endDate);
        }

        public List<productSalesReport> getCustomerServiceSalesValueCash(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerServiceSalesValueCash(startDate, endDate);
        }

        public List<productSalesReport> getCustomerServiceSalesValueAll(DateTime startDate, DateTime endDate)
        {
            return db.getCustomerServiceSalesValueAll(startDate, endDate);
        }
        #endregion

        #region Top Products
        public List<productSalesReport> getProductSalesVolumeAll(DateTime startDate, DateTime endDate)
        {
            return db.getProductSalesVolumeAll(startDate, endDate);
        }

        public List<productSalesReport> getProductSalesVolumeCredit(DateTime startDate, DateTime endDate)
        {
            return db.getProductSalesVolumeCredit(startDate, endDate);
        }

        public List<productSalesReport> getProductSalesVolumeCash(DateTime startDate, DateTime endDate)
        {
            return db.getProductSalesVolumeCash(startDate, endDate);
        }

        public List<productSalesReport> getProductSalesValueCredit(DateTime startDate, DateTime endDate)
        {
            return db.getProductSalesValueCredit(startDate, endDate);
        }

        public List<productSalesReport> getProductSalesValueCash(DateTime startDate, DateTime endDate)
        {
            return db.getProductSalesValueCash(startDate, endDate);
        }

        public List<productSalesReport> getProductSalesValueAll(DateTime startDate, DateTime endDate)
        {
            return db.getProductSalesValueAll(startDate, endDate);
        }
        #endregion

        #region Top Service
        public List<productSalesReport> getServiceSalesVolumeAll(DateTime startDate, DateTime endDate)
        {
            return db.getServiceSalesVolumeAll(startDate, endDate);
        }

        public List<productSalesReport> getServiceSalesVolumeCredit(DateTime startDate, DateTime endDate)
        {
            return db.getServiceSalesVolumeCredit(startDate, endDate);
        }

        public List<productSalesReport> getServiceSalesVolumeCash(DateTime startDate, DateTime endDate)
        {
            return db.getServiceSalesVolumeCash(startDate, endDate);
        }

        public List<productSalesReport> getServiceSalesValueCredit(DateTime startDate, DateTime endDate)
        {
            return db.getServiceSalesValueCredit(startDate, endDate);
        }

        public List<productSalesReport> getServiceSalesValueCash(DateTime startDate, DateTime endDate)
        {
            return db.getServiceSalesValueCash(startDate, endDate);
        }

        public List<productSalesReport> getServiceSalesValueAll(DateTime startDate, DateTime endDate)
        {
            return db.getServiceSalesValueAll(startDate, endDate);
        }
        #endregion

        public List<productSalesReport> getSalesGauge(string ProductID)
        {
            return db.getSalesGauge(ProductID);
        }
        public List<SP_TotalBksMissedByCustomers> returnTotalbksMissedbyCustomers(DateTime startDate, DateTime endDate)
        {
            return db.returnTotalbksMissedbyCustomers(startDate, endDate);
        }
        public List<SP_GetReviews> mostPopularStylist(DateTime startDate, DateTime endDate)
        {
            return db.mostPopularStylist(startDate, endDate);
        }
        public List<SP_GetReviews> customerSatistfaction(DateTime startDate, DateTime endDate)
        {
            return db.customerSatistfaction(startDate, endDate);
        }
        #endregion

        public USER getManagerContact()
        {
            return db.getManagerContact();
        }

        public List<SP_GetTodaysBookings> getTodaysBookings() 
        {
            return db.getTodaysBookings();
        }

        public SP_ViewStylistSpecialisationAndBio viewStylistSpecialisationAndBio(string empID)
        {
            return db.viewStylistSpecialisationAndBio(empID);
        }

        public SP_ViewEmployee viewEmployee(string empID)
        {
            return db.viewEmployee(empID);
        }

        public BUSINESS getBusinessTable()
        {
            return db.getBusinessTable();
        }

        public bool addPaymentTypeToSalesRecord(string paymentType, string saleID)
        {
            return db.addPaymentTypeToSalesRecord(paymentType, saleID);
        }

        public string getSalePaymentType(String SaleID)
        {
            return db.getSalePaymentType(SaleID);
        }

        public bool createSalesDTLRecord(SALES_DTL detailLine)
        {
            return db.createSalesDTLRecord(detailLine);
        }

        public SP_CheckForUserType BLL_CheckForUserType(string id)
        {
            return db.CheckForUserType(id);
        }

        public SP_AddUser BLL_AddUser(USER user)
        {
            return db.AddUser(user);
        }

        public Tuple<List<SP_GetAllAccessories>, List<SP_GetAllTreatments>> getAllProductsAndDetails()
        {
            return db.getAllProductsAndDetails();
        }

        public USER GetUserDetails(string ID)
        {
            return db.GetUserDetails(ID);
        }

        public SP_GetCurrentVATate GetVATRate()
        {
            return db.GetVATRate();
        }

        public List<SP_GetCustomerBooking> getCustomerUpcomingBookings(string CustomerID)
        {
            return db.getCustomerUpcomingBookings(CustomerID);
        }

        public SP_GetCustomerBooking getCustomerUpcomingBookingDetails(string BookingID)
        {
            return db.getCustomerUpcomingBookingDetails(BookingID);
        }

        public bool deleteBooking(string BookingID)
        {
            return db.deleteBooking(BookingID);
        }

        public List<SP_GetCustomerBooking> getCustomerPastBookings(string CustomerID)
        {
            return db.getCustomerPastBookings(CustomerID);
        }

        public List<SP_GetEmpNames> BLL_GetEmpNames()
        {
            return db.GetEmpNames();
        }

        public List<SP_GetEmpAgenda> BLL_GetEmpAgenda(string employeeID, DateTime bookingDate, string sortBy, string sortDir)
        {
            return db.GetEmpAgenda(employeeID, bookingDate,sortBy,sortDir);
        }

        public SP_GetCustomerBooking getCustomerPastBookingDetails(string BookingID)
        {
            return db.getCustomerPastBookingDetails(BookingID);
        }

        public List<SP_getInvoiceDL> getInvoiceDL(string BookingID)
        {
            return db.getInvoiceDL(BookingID);
        }

        public EMPLOYEE getEmployeeType(string EmployeeID)
        {
            return db.getEmployeeType(EmployeeID);
        }

        public bool updateBooking(BOOKING bookingUpdate)
        {
            return db.updateBooking(bookingUpdate);
        }

        public bool updateUser(USER userUpdate)
        {
            return db.updateUser(userUpdate);
        }
        public bool BLL_CheckIn(BOOKING booking)
        {
            return db.CheckIn(booking);
        }
        public SP_GetAllofBookingDTL BLL_GetAllofBookingDTL(string bookingID, string customerID)
        {
            return db.GetAllofBookingDTL(bookingID, customerID);
        }
        public SP_GetBookingServiceDTL BLL_GetBookingServiceDTL(string bookingID, string customerID)
        {
            return db.GetBookingServiceDTL(bookingID, customerID);
        }
        public SP_ViewCustVisit BLL_ViewCustVisit(string customerID, string bookingID)
        {
            return db.ViewCustVisit(customerID, bookingID);
        }
        public bool BLL_UpdateCustVisit(CUST_VISIT visit, BOOKING b)
        {
            return db.UpdateCustVisit(visit,b);
        }
        public bool BLL_CreateCustVisit(CUST_VISIT cust_visit)
        {
            return db.CreateCustVisit(cust_visit);
        }

         public bool BLL_AddBooking(BOOKING addBooking)
        {
            return db.AddBooking(addBooking);
        }

        public List<SP_GetStylists> BLL_GetStylists()
        {
            return db.GetAllStylists();
        }
        public List<SP_GetServices> BLL_GetAllServices()
        {
            return db.GetAllServices();
        }
         public List<SP_GetSlotTimes> BLL_GetAllTimeSlots()
        {
            return db.GetAllTimeSlots();
        }
        public List<SP_GetBookedTimes> BLL_GetBookedStylistTimes(string stylistID, DateTime bookingDate)
        {
            return db.GetBookedStylistTimes(stylistID, bookingDate);
        }

        public List<SP_GetMyNextCustomer> BLL_GetMyNextCustomer(string employeeID, DateTime bookingDate)
        {
            return db.GetMyNextCustomer(employeeID, bookingDate);
        }

        public SP_GetCustomerBooking getBookingDetaisForCheckOut(string BookingID)
        {
            return db.getBookingDetaisForCheckOut(BookingID);
        }

        public bool createSalesRecordForBooking(string bookingID)
        {
            return db.createSalesRecordForBooking(bookingID);
        }

        public bool updateVatRate(string bussinesID, int vatRate)
        {
            return db.updateVatRate(bussinesID, vatRate);
        }

        public bool updateVatRegNo(string bussinesID, string vatRegNo)
        {
            return db.updateVatRegNo(bussinesID, vatRegNo);
        }

        public bool updateAddress(string bussinesID, string addresLine1, string addressLine2)
        {
            return db.updateAddress(bussinesID, addresLine1, addressLine2);
        }

        public bool updateWeekdayHours(string bussinesID, DateTime wDStart, DateTime wDEnd)
        {
            return db.updateWeekdayHours(bussinesID, wDStart, wDEnd);
        }

        public bool updateWeekendHours(string bussinesID, DateTime wEStart, DateTime wEEnd)
        {
            return db.updateWeekendHours(bussinesID, wEStart, wEEnd);
        }

        public bool updatePublicHolidayHours(string bussinesID, DateTime pHStart, DateTime pHEnd)
        {
            return db.updatePublicHolidayHours(bussinesID, pHStart, pHEnd);
        }

        public bool updatePhoneNumber(string bussinesID, string PhoneNumber)
        {
            return db.updatePhoneNumber(bussinesID, PhoneNumber);
        }

        public List<SP_ViewEmployee> viewAllEmployees()
        {
            return db.viewAllEmployees();
        }

        public List<SP_GetEmployeeTypes> getEmpTypes()
        {
            return db.getEmpTypes();
        }

        public List<PRODUCT> getAllProducts()
        {
            return db.getAllProducts();
        }
        

        public List<ProductType> getProductTypes()
        {
            return db.getProductTypes();
        }

        public List<SP_UserList> userList()
        {
            return db.userList();
        }

        public bool addEmployee(string empID, string bio, string ad1, string ad2, string suburb, string city, string firstname
                                , string lastname, string username, string email, string contactNo, string password,
                                string userimage, string passReset)
        {
            return db.addEmployee(empID,bio,ad1,ad2,suburb,city,firstname,lastname
                                    ,username,email,contactNo,password,userimage,passReset);
        }

        public bool updateEmployee(EMPLOYEE emp)
        {
            return db.updateEmployee(emp);
        }

        public List<SP_BookingsReportForHairstylist> getBookingsReportForHairstylist(string stylistID)
        {
            return db.getBookingsReportForHairstylist(stylistID);
        }

        public List<SP_BookingsReportForHairstylist> getBookingReportForHairstylistWithDateRange(string stylistID, DateTime startDate, DateTime endDate)
        {
          return db.getBookingReportForHairstylistWithDateRange(stylistID, startDate, endDate);

        }

        public List<SP_SaleOfHairstylist> getSaleOfHairstylist(string stylistID, DateTime startDate, DateTime endDate)
        {
            return db.getSaleOfHairstylist(stylistID, startDate,endDate);

        }
                public List<SP_GetStylists> BLL_GetAllStylists()
        {
            return db.GetAllStylists();
        }

        public List<SP_GetStylistBookings> getStylistPastBookings(string empID, string sortBy, string sortDir)
        {
            return db.getStylistPastBookings(empID,sortBy,sortDir);
        }

        public List<SP_GetStylistBookings> getStylistPastBookingsDateRange(string empID, DateTime startDate, DateTime endDate, string sortBy, string sortDir)
        {
            return db.getStylistPastBookingsDateRange(empID,startDate,endDate,sortBy,sortDir);
        }

        public List<SP_GetStylistBookings> getStylistUpcomingBookings(string empID, string sortBy, string sortDir)
        {
            return db.getStylistUpcomingBookings(empID,sortBy,sortDir);
        }

        public List<SP_AboutStylist> aboutStylist()
        {
            return db.aboutStylist();
        }

        public List<SP_GetStylistBookings> getStylistUpcomingBookingsDR(string empID, DateTime startDate, DateTime endDate, string sortBy, string sortDir)
        {
            return db.getStylistUpcomingBookingsDR(empID, startDate, endDate,sortBy,sortDir);
        }

        public List<SP_GetStylistBookings> getAllStylistsUpcomingBksForDate(DateTime bookingDate, string sortBy, string sortDir)
        {
            return db.getAllStylistsUpcomingBksForDate(bookingDate,sortBy,sortDir);
        }
       
        public List<SP_GetStylistBookings> getAllStylistsUpcomingBookings(string sortBy, string sortDir)
        {
            return db.getAllStylistsUpcomingBookings(sortBy,sortDir);
        }
        
        public List<SP_GetStylistBookings> getAllStylistsPastBookings(string sortBy, string sortDir)
        {
            return db.getAllStylistsPastBookings(sortBy,sortDir);
        }

        public List<SP_GetStylistBookings> getAllStylistsPastBksForDate(DateTime date, string sortBy, string sortDir)
        {
            return db.getAllStylistsPastBksForDate(date,sortBy,sortDir);
        }
        public List<SP_GetStylistBookings> getAllStylistsUpcomingBksDR(DateTime startDate, DateTime endDate, string sortBy, string sortDir)
        {
            return db.getAllStylistsUpcomingBksDR(startDate,endDate,sortBy,sortDir);
        }
        public List<SP_GetStylistBookings> getAllStylistsPastBookingsDateRange(DateTime startDate, DateTime endDate, string sortBy, string sortDir)
        {
            return db.getAllStylistsPastBookingsDateRange(startDate,endDate,sortBy,sortDir);
        }

        public List<SP_GetStylistBookings> getStylistPastBksForDate(string empID, DateTime day, string sortBy, string sortDir)
        {
            return db.getStylistPastBksForDate(empID, day,sortBy,sortDir);
        }
        public List<SP_GetStylistBookings> getStylistUpcomingBkForDate(string empID, DateTime day, string sortBy, string sortDir)
        {
            return db.getStylistUpcomingBkForDate(empID, day, sortBy, sortDir);
        }
        public bool BLL_AddToBookingService(BookingService bs)
        {
            return db.AddToBookingService(bs);
        }
        public bool updateService(PRODUCT p, SERVICE s)
        {
            return db.updateService(p, s);
        }
        public SERVICE BLL_GetSlotLength(string serviceID)
        {
            return db.GetSlotLength(serviceID);
        }
        public SP_GetEmployee_S_ getEmployee_S(string stylistID)
        {
            return db.getEmployee_S(stylistID);
        }
        public bool addSpecialisation(STYLIST_SERVICE ss)
        {
            return db.addSpecialisation(ss);
        }
        public SP_GetEmployee_S_ getBio(string id)
        {
            return db.getBio(id);
        }
         public List<SP_GetTopCustomerbyBooking> getTopCustomerByBookings(DateTime startDate, DateTime endDate)
        {
            return db.getTopCustomerByBookings(startDate, endDate);
        }
        public List<SP_GetStylistBookings> getCustomerPastBookingsForDate(string customerID, DateTime day)
        {
            return db.getCustomerPastBookingsForDate(customerID, day);
        }
        public List<SP_GetLeaveServices> BLL_GetLeaveServices()
        {
            return db.GetLeaveServices();
        }
        public bool BLL_UpdateOrder(string orderID, DateTime dateReceived, bool received)
        {
            return db.UpdateOrder(orderID, dateReceived, received);
        }
        public bool BLL_UpdateQtyOnHand(string prodID, int qty)
        {
            return db.UpdateQtyOnHand(prodID, qty);
        }
        public SP_ReturnBooking returnNextBooking(string startTime, string bookingID, string stylistID, DateTime date)
        {
            return db.returnNextBooking(startTime, bookingID, stylistID, date);
        }
        public SP_ReturnBooking returnBooking(string bookingID, string customerID, string stylistID, DateTime date)
        {
            return db.returnBooking(bookingID, customerID, stylistID, date);
        }
        public List<SP_ReturnAvailServices> returnAvailServices(int num)
        {
            return db.returnAvailServices(num);
        }
        public SP_GetBookingServices getLeaveReason(string BookingID)
        {
            return db.getLeaveReason(BookingID);
        }
    }
}

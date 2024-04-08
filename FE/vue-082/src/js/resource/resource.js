const Text = {
  AddNew: "Thêm mới",
  Employee: "Nhân viên",
  Selected: "Đã chọn",
  Removed: "Bỏ chọn",
  RemoveAll: "Xóa tất cả",
  SearchField: "Tìm theo mã, tên nhân viên",
  Excel: "Excel",
  Import: "Nhập khẩu",
  ExportAll: "Xuất khẩu toàn bộ",
  ExportList: "Xuất khẩu theo danh sách",
  Setting: "Cài đặt",
  ChangeColumns: "Tùy chỉnh cột",
  Search: "Tìm kiếm",
  DefaultSetting: "Đặt lại mặc định",
  RefreshData: "Load lại dữ liệu",
  Edit: "Chỉnh sửa",
  Open: "Mở rộng",
  Delete: "Xóa bản ghi",
  Clone: "Nhân bản",
  NoDataContent: "Không có bản ghi nào phù hợp.",
  IdentityNumber: "Số chứng minh nhân dân",
  Total: "Tổng",
  Record: "Bản ghi",
  RecordPerPage: "Số bản ghi/trang",
  SelectValue: "- Chọn giá trị -",
  Confirm: "Xác nhận",
  Cancel: "Hủy",
  SaveAndAdd: "Cất và thêm",
  Save: "Cất",
  EditInfo: "Chỉnh sửa thông tin",
  Gender: "Giới tính",
  SelectValueToImport: "Chọn dữ liệu đã chuẩn bị để nhập khẩu vào phần mềm.",
  SelectAFile: "Chọn một tệp để nhập khẩu.",
  Select: "Chọn",
  NoFileTemplate:
    "Chưa có tệp mẫu để chọn dữ liệu? Tải tệp excel mẫu mà phần mềm cung cấp để chuẩn bị dữ liệu nhập khẩu",
  Here: "Tại đây.",
  ResultImport: "Kết quả nhập khẩu",
  DownLoadFile: "Tải về tệp tin",
  DownLoadFileImportErorr:
    "Tải về tệp tin các dòng nhập khẩu không thành công",
  CountRowImportSuccess: "Số dòng nhập khẩu thành công",
  CountRowImportFailure: "Số dòng nhập khẩu thất bại",
  CountValid: "dòng hợp lệ",
  CountIsValid: "dòng không hợp lệ",
  Status: "Tình trạng",
  Correct: "Hợp lệ",
  FileNameExcel: "Danh sách nhân viên.xlsx",
  ForgotPassword: "Quên mật khẩu?",
  Login: "Đăng nhập",
  LoginWith:"Hoặc đăng nhập với",
  AccessTokenExpired: "Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại.",
  UsernameLoginNotEmpty: "Tên đăng nhập không được để trống.",
  PasswordLoginNotEmpty: "Mật khẩu không được để trống.",
  Have: "Có",
  No: "Không",
};
const ExcelImport = {
  Step: "Bước",
  StepOne: "1. Chọn tệp nguồn",
  StepSecond: "2. Kiểm tra dữ liệu",
  StepThird: "3. Kết quả nhập khẩu",
  SelectFile: "Chọn tệp",
  GetFileTemplate: "Tải tệp mẫu",
  Help: "Giúp",
  Prev:"Quay lại",
  Next: "Tiếp tục",
  Perform: "Thực hiện",
  Cancle: "Hủy bỏ",
  Close: "Đóng",
  FileNotEmpty: "File không được để trống."
};
const ToolTip = {
  Help: "Trợ giúp",
  ESC: "Đóng (ESC)",
};
const EmployeeField = {
  EmployeeCode: "Mã nhân viên",
  EmployeeName: "Tên nhân viên",
  Gender: "Giới tính",
  DateOfBirth: "Ngày sinh",
  IdentityNumber: "Số CMND",
  IdentityDate: "Ngày cấp",
  IdentityPlace: "Nơi cấp",
  EmployeePosition: "Chức danh",
  DepartmentName: "Đơn vị",
  Address: "Địa chỉ",
  PhoneNumber: "ĐT di động",
  TelephoneNumberToolTip: "Điện thoại di động",
  PhoneNumberFixed: "ĐT cố định",
  PhoneNumberFixedToolTip: "Điện thoại cố định",
  Email: "Email",
  BankAccount: "Số tài khoản",
  BankName: "Tên ngân hàng",
  BankAddress: "Chi nhánh ngân hàng",
  Employee: "Nhân viên",
  
};

const Resource = {
  Language: {
    VN: "VN",
    EN: "EN",
  },
  VN: {
    Customer: "khách hàng",
    Employee: "Nhân viên",
    CustomerProperty: {
      CustomerCode: "Mã khách hàng",
      FullName: "Họ và tên",
      Email: "Email",
    },
    EmailNotValid: "Email không đúng định dạng.",
    Gender: {
      Male: "Nam",
      Female: "Nữ",
      Other: "Khác",
    },
    ErrorMessage(msg) {
      return `${msg} không được để trống.`;
    },
    ToastMessage: {
      Type: {
        Success: "success",
        Error: "error",
        Warning: "warning",
        Info: "info",
      },
      Status: {
        Success: "Thành công!",
        Error: "Lỗi!",
        Warning: "Cảnh báo!",
        Info: "Thông tin!",
      },
    },
    AddSuccess: "Thêm thành công.",
    EditSuccess: "Sửa thành công.",
    DeleteSuccess: "Xóa thành công.",
    Dialog: {
      Type: {
        Question: "question",
        Warning: "warning",
        Info: "info",
      },
    },
    ConfirmText(msg) {
      return `Bạn có chắc chắn xóa ${msg} không?`;
    },
    ConfirmDeleteAll(msg) {
      return `Bạn có chắc chắn xóa những ${msg} đã chọn này?`;
    },
    Warning: "Cảnh báo",
    ListIdsDeleteEmpty: "Hãy chọn ít nhất 1 khách hàng để xuất khẩu.",
    DeleteConfirm: "Xác nhận xóa",
    NoticeMessage: "Thông báo",
    WanrningMessage: "Cảnh báo",
    ElementNotFound: "Không tìm thấy phần tử nào",
    NotHaveAccess: "Không có quyền truy cập!",
    InCorrectAccount: "Thông tin tài khoản không chính xác.",
    InCorrectUrlLink: "Đường dẫn không chính xác!",
    MessageHelp: "Vui lòng liên hệ MISA để được hỗ trợ.",
    Text,
    EmployeeField,
    ToolTip,
    ExcelImport
  },
  EN: {
    Customer: "customer",
    EmailNotValid: "Email is not valid",
    Gender: {
      Male: "Male",
      Female: "Female",
      Other: "Khác",
    },
    ErrorMessage(msg) {
      return `${msg} is not empty`;
    },
    ToastMessage: {
      Type: {
        Success: "success",
        Error: "error",
        Warning: "warning",
        Info: "info",
      },
      Status: {
        Success: "Success!",
        Error: "Error!",
        Warning: "Warning!",
        Info: "Info!",
      },
    },
    AddSuccess: "Add Success",
    EditSuccess: "Add Success",
    DeleteSuccess: "Add Success",
    Dialog: {
      Type: {
        Question: "question",
        Warning: "warning",
        Info: "info",
      },
    },
    ConfirmText(msg) {
      return `Are you sure delete this ${msg}?`;
    },
    DeleteConfirm: "Delete Confirm",
    NoticeMessage: "Notification",
  },
};

export default Resource;

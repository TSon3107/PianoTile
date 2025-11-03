using UnityEngine;
using TMPro;
using Firebase.Auth;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    [Header("Login/Register UI")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    [Header("Confirm Name Canvas")]
    public GameObject confirmNameCanvas;   // Canvas “Confirm name”
    public TMP_InputField nameInput;
    public TMP_Text warningText;

    // ============================
    //  ĐĂNG NHẬP
    // ============================
    public async void Login()
    {
        string email = emailField.text.Trim();
        string pass = passwordField.text.Trim();

        if (FirebaseInit.Auth == null)
        {
            Debug.LogError("❌ Firebase chưa khởi tạo!");
            return;
        }

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
        {
            Debug.LogWarning("❗ Vui lòng nhập email và mật khẩu.");
            return;
        }

        try
        {
            var result = await FirebaseInit.Auth.SignInWithEmailAndPasswordAsync(email, pass);
            FirebaseInit.User = result.User;
            Debug.Log("✅ Đăng nhập thành công: " + FirebaseInit.User.Email);

            // Nếu chưa có DisplayName, yêu cầu nhập tên
            if (string.IsNullOrEmpty(FirebaseInit.User.DisplayName))
            {
                ShowNamePopup();
            }
            else
            {
                Debug.Log("🎮 Tên người chơi: " + FirebaseInit.User.DisplayName);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Lỗi đăng nhập: " + ex.Message);
        }
    }

    // ============================
    //  ĐĂNG KÝ MỚI
    // ============================
    public async void Register()
    {
        string email = emailField.text.Trim();
        string pass = passwordField.text.Trim();

        if (FirebaseInit.Auth == null)
        {
            Debug.LogError("❌ Firebase chưa khởi tạo!");
            return;
        }

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
        {
            Debug.LogWarning("❗ Vui lòng nhập email và mật khẩu.");
            return;
        }

        try
        {
            var newUser = await FirebaseInit.Auth.CreateUserWithEmailAndPasswordAsync(email, pass);
            FirebaseInit.User = newUser.User;
            Debug.Log("🆕 Tạo tài khoản thành công: " + FirebaseInit.User.Email);

            // Luôn yêu cầu nhập tên sau khi tạo tài khoản
            ShowNamePopup();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Lỗi đăng ký: " + ex.Message);
        }
    }

    // ============================
    //  XỬ LÝ CONFIRM NAME
    // ============================
    void ShowNamePopup()
    {
        if (confirmNameCanvas == null)
        {
            Debug.LogError("❌ confirmNameCanvas chưa được gán trong Inspector!");
            return;
        }

        confirmNameCanvas.SetActive(true);

        if (warningText != null)
            warningText.text = "";
    }

    public async void ConfirmName()
    {
        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            if (warningText != null)
                warningText.text = "Tên không được để trống!";
            return;
        }

        // Cập nhật DisplayName trên Firebase
        UserProfile profile = new UserProfile { DisplayName = playerName };
        await FirebaseInit.User.UpdateUserProfileAsync(profile);

        Debug.Log("✅ Tên người chơi đã lưu: " + playerName);
        confirmNameCanvas.SetActive(false);
    }
}

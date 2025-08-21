function Login() {
  return (
    <div className="login-page">
      <form className="login-form">
        <label name="login-email">Login Email</label>
        <input name="login-email" />
        <label name="login-password"></label>
        <input name="login-password" />
      </form>
    </div>
  );
}

export default Login;

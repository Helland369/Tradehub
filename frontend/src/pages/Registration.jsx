function Registration() {
  return (
    <div className="registration">
      <form name="registration-form">
        <label name="reg-fname">First Name</label>
        <input name="reg-fname" type="text" />
        <label name="reg-lname">Last Name</label>
        <input name="reg-lname" type="text" />
        <label name="reg-email">Email</label>
        <input name="reg-email" type="email" />
        <label name="reg-uname"> User name</label>
        <input name="reg-uname" type="text" />
        <label name="reg-street">Street</label>
        <input name="reg-street" type="text" />
        <label name="reg-city">City</label>
        <input name="reg-city" type="text" />
        <label name="reg-zip">Zip code</label>
        <input name="reg-zip" type="number" />
        <label name="reg-country">Country</label>
        <input name="reg-country" type="text" />
        <label name="reg-phone">Phone number</label>
        <input name="reg-phone" type="text" />
        <label name="reg-password">Password</label>
        <input name="reg-password" type="password" />
      </form>
    </div>
  );
}

export default Registration;

# Conference Management API

## Tech Stack

* ASP.NET Core 8
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* Role-Based Authorization

---

## Core Features

### Authentication & Authorization

* User Registration
* User Login
* JWT Authentication
* Role-Based Authorization
* User Roles:

  * Admin
  * User
  * Promoter

### Conference Management

* Create Conference
* Update Conference
* Delete Conference
* Get Conference Details
* Conference Ownership Authorization

### Registration Management

* Conference Registration
* Registration Confirmation
* Registration Status Tracking
* Admin Registration Management

---

## Promoter Referral System

### Overview

The platform supports promoter-driven conference registrations through unique referral codes.

### Promoter Assignment

* Only administrators can assign promoter privileges.
* A promoter must be an existing user.
* Each promoter has a unique referral code.
* Each promoter owns a single promoter profile.

### Registration Flow

Users can register for conferences with or without a referral code.

#### Registration Without Referral Code

* Registration is created successfully.
* No promoter is associated with the registration.

#### Registration With Invalid Referral Code

* Registration is created successfully.
* No promoter is associated with the registration.
* The API returns a warning message.

#### Registration With Valid Referral Code

* Registration is created successfully.
* The registration is linked to the promoter profile.
* The promoter can later receive statistics and commission calculations based on associated registrations.

---

## Database Relationships

### User ↔ PromoterProfile

```text
User (1) <-> (1) PromoterProfile
```

### PromoterProfile ↔ Registration

```text
PromoterProfile (1) -> (*) Registration
```

### User ↔ Registration

```text
User (1) -> (*) Registration
```

### Conference ↔ Registration

```text
Conference (1) -> (*) Registration
```

---

## Registration API Response

The registration endpoint returns:

* Registration success status
* Promoter association status
* Promoter name (if available)
* Optional warning messages

### Example Response

```json
{
  "success": true,
  "hasPromoter": true,
  "promoterName": "TestUser",
  "warning": null
}
```

File & Image Management
User Profile Images
* Authenticated users can upload a profile image
* Images are uploaded directly through the API (multipart/form-data)
* Files are stored in wwwroot/images
* Each file is saved with a unique GUID-based filename
* The generated image URL is stored in the user profile (ProfileImageUrl)
* Users can update their own profile image via /me/profile-image
* Administrators can update profile images for users (if extended in future)

* Conference Images
* Conference owners and administrators can upload a conference image
* Images are uploaded directly through the API (multipart/form-data)
* Files are stored in wwwroot/images
* Each file is saved with a unique GUID-based filename
* The generated image URL is stored in the conference (ImageUrl)
* Ownership validation ensures only:
  * Conference owner OR
  * Administrator
* Endpoint: PUT /api/conference/{id}/image


Storage Strategy
* All uploaded files are stored locally in wwwroot/images
* Filenames are generated using GUID to avoid collisions
* Static file middleware exposes images via URL access
* URLs are stored in the database and returned via API responses
 
Database Fields

User
* ProfileImageUrl – stores the profile image URL

Conference
* ImageUrl – stores the conference image URL

Example Image URL
* /images/b3aa66ff-4350-440c-a702-d9632823e501.png
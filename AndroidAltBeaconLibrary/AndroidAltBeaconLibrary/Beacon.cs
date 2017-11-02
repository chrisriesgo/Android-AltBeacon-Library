using System;
using Android.Runtime;

namespace AltBeaconOrg.BoundBeacon
{
    public partial class Beacon
    {
            static Delegate cb_describeContents;
#pragma warning disable 0169
        static Delegate GetDescribeContentsHandler()
        {
            if (cb_describeContents == null)
                cb_describeContents = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int>)n_DescribeContents);
            return cb_describeContents;
        }

        static int n_DescribeContents(IntPtr jnienv, IntPtr native__this)
        {
            global::AltBeaconOrg.BoundBeacon.Region __this = global::Java.Lang.Object.GetObject<global::AltBeaconOrg.BoundBeacon.Region>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            return __this.DescribeContents();
        }
#pragma warning restore 0169

        static IntPtr id_describeContents;
        // Metadata.xml XPath method reference: path="/api/package[@name='org.altbeacon.beacon']/class[@name='Region']/method[@name='describeContents' and count(parameter)=0]"
        [Register("describeContents", "()I", "GetDescribeContentsHandler")]
        public virtual unsafe int DescribeContents()
        {
            if (id_describeContents == IntPtr.Zero)
                id_describeContents = JNIEnv.GetMethodID(class_ref, "describeContents", "()I");
            try
            {

                if (((object)this).GetType() == ThresholdType)
                    return JNIEnv.CallIntMethod(((global::Java.Lang.Object)this).Handle, id_describeContents);
                else
                    return JNIEnv.CallNonvirtualIntMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "describeContents", "()I"));
            }
            finally
            {
            }
        }


        static Delegate cb_writeToParcel_Landroid_os_Parcel_I;
#pragma warning disable 0169
        static Delegate GetWriteToParcel_Landroid_os_Parcel_IHandler()
        {
            if (cb_writeToParcel_Landroid_os_Parcel_I == null)
                cb_writeToParcel_Landroid_os_Parcel_I = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr, int>)n_WriteToParcel_Landroid_os_Parcel_I);
            return cb_writeToParcel_Landroid_os_Parcel_I;
        }

        static void n_WriteToParcel_Landroid_os_Parcel_I(IntPtr jnienv, IntPtr native__this, IntPtr native__out, int native_flags)
        {
            global::AltBeaconOrg.BoundBeacon.Region __this = global::Java.Lang.Object.GetObject<global::AltBeaconOrg.BoundBeacon.Region>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Android.OS.Parcel @out = global::Java.Lang.Object.GetObject<global::Android.OS.Parcel>(native__out, JniHandleOwnership.DoNotTransfer);
            global::Android.OS.ParcelableWriteFlags flags = (global::Android.OS.ParcelableWriteFlags)native_flags;
            __this.WriteToParcel(@out, flags);
        }
#pragma warning restore 0169

        static IntPtr id_writeToParcel_Landroid_os_Parcel_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='org.altbeacon.beacon']/class[@name='Region']/method[@name='writeToParcel' and count(parameter)=2 and parameter[1][@type='android.os.Parcel'] and parameter[2][@type='int']]"
        [Register("writeToParcel", "(Landroid/os/Parcel;I)V", "GetWriteToParcel_Landroid_os_Parcel_IHandler")]
        public virtual unsafe void WriteToParcel(global::Android.OS.Parcel @out, [global::Android.Runtime.GeneratedEnum] global::Android.OS.ParcelableWriteFlags flags)
        {
            if (id_writeToParcel_Landroid_os_Parcel_I == IntPtr.Zero)
                id_writeToParcel_Landroid_os_Parcel_I = JNIEnv.GetMethodID(class_ref, "writeToParcel", "(Landroid/os/Parcel;I)V");
            try
            {
                JValue* __args = stackalloc JValue[2];
                __args[0] = new JValue(@out);
                __args[1] = new JValue((int)flags);

                if (((object)this).GetType() == ThresholdType)
                    JNIEnv.CallVoidMethod(((global::Java.Lang.Object)this).Handle, id_writeToParcel_Landroid_os_Parcel_I, __args);
                else
                    JNIEnv.CallNonvirtualVoidMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "writeToParcel", "(Landroid/os/Parcel;I)V"), __args);
            }
            finally
            {
            }
        }
    }
}

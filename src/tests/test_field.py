# -*- coding: utf-8 -*-

import unittest

import System
from Python.Test import FieldTest


class FieldTests(unittest.TestCase):
    """Test CLR field support."""

    def test_public_instance_field(self):
        """Test public instance fields."""
        ob = FieldTest()
        self.assertTrue(ob.PublicField == 0)

        ob.PublicField = 1
        self.assertTrue(ob.PublicField == 1)

        def test():
            del FieldTest().PublicField

        self.assertRaises(TypeError, test)

    def test_public_static_field(self):
        """Test public static fields."""
        ob = FieldTest()
        self.assertTrue(FieldTest.PublicStaticField == 0)

        FieldTest.PublicStaticField = 1
        self.assertTrue(FieldTest.PublicStaticField == 1)

        self.assertTrue(ob.PublicStaticField == 1)
        ob.PublicStaticField = 0
        self.assertTrue(ob.PublicStaticField == 0)

        def test():
            del FieldTest.PublicStaticField

        self.assertRaises(TypeError, test)

        def test():
            del FieldTest().PublicStaticField

        self.assertRaises(TypeError, test)

    def test_protected_instance_field(self):
        """Test protected instance fields."""
        ob = FieldTest()
        self.assertTrue(ob.ProtectedField == 0)

        ob.ProtectedField = 1
        self.assertTrue(ob.ProtectedField == 1)

        def test():
            del FieldTest().ProtectedField

        self.assertRaises(TypeError, test)

    def test_protected_static_field(self):
        """Test protected static fields."""
        ob = FieldTest()
        self.assertTrue(FieldTest.ProtectedStaticField == 0)

        FieldTest.ProtectedStaticField = 1
        self.assertTrue(FieldTest.ProtectedStaticField == 1)

        self.assertTrue(ob.ProtectedStaticField == 1)
        ob.ProtectedStaticField = 0
        self.assertTrue(ob.ProtectedStaticField == 0)

        def test():
            del FieldTest.ProtectedStaticField

        self.assertRaises(TypeError, test)

        def test():
            del FieldTest().ProtectedStaticField

        self.assertRaises(TypeError, test)

    def test_read_only_instance_field(self):
        """Test readonly instance fields."""
        self.assertTrue(FieldTest().ReadOnlyField == 0)

        def test():
            FieldTest().ReadOnlyField = 1

        self.assertRaises(TypeError, test)

        def test():
            del FieldTest().ReadOnlyField

        self.assertRaises(TypeError, test)

    def test_read_only_static_field(self):
        """Test readonly static fields."""
        ob = FieldTest()

        self.assertTrue(FieldTest.ReadOnlyStaticField == 0)
        self.assertTrue(ob.ReadOnlyStaticField == 0)

        def test():
            FieldTest.ReadOnlyStaticField = 1

        self.assertRaises(TypeError, test)

        def test():
            FieldTest().ReadOnlyStaticField = 1

        self.assertRaises(TypeError, test)

        def test():
            del FieldTest.ReadOnlyStaticField

        self.assertRaises(TypeError, test)

        def test():
            del FieldTest().ReadOnlyStaticField

        self.assertRaises(TypeError, test)

    def test_constant_field(self):
        """Test const fields."""
        ob = FieldTest()

        self.assertTrue(FieldTest.ConstField == 0)
        self.assertTrue(ob.ConstField == 0)

        def test():
            FieldTest().ConstField = 1

        self.assertRaises(TypeError, test)

        def test():
            FieldTest.ConstField = 1

        self.assertRaises(TypeError, test)

        def test():
            del FieldTest().ConstField

        self.assertRaises(TypeError, test)

        def test():
            del FieldTest.ConstField

        self.assertRaises(TypeError, test)

    def test_internal_field(self):
        """Test internal fields."""

        def test():
            f = FieldTest().InternalField

        self.assertRaises(AttributeError, test)

        def test():
            f = FieldTest().InternalStaticField

        self.assertRaises(AttributeError, test)

        def test():
            f = FieldTest.InternalStaticField

        self.assertRaises(AttributeError, test)

    def test_private_field(self):
        """Test private fields."""

        def test():
            f = FieldTest().PrivateField

        self.assertRaises(AttributeError, test)

        def test():
            f = FieldTest().PrivateStaticField

        self.assertRaises(AttributeError, test)

        def test():
            f = FieldTest.PrivateStaticField

        self.assertRaises(AttributeError, test)

    def test_field_descriptor_get_set(self):
        """Test field descriptor get / set."""

        # This test ensures that setting an attribute implemented with
        # a descriptor actually goes through the descriptor (rather than
        # silently replacing the descriptor in the instance or type dict.

        ob = FieldTest()

        self.assertTrue(FieldTest.PublicStaticField == 0)
        self.assertTrue(ob.PublicStaticField == 0)

        descriptor = FieldTest.__dict__['PublicStaticField']
        self.assertTrue(type(descriptor) != int)

        ob.PublicStaticField = 0
        descriptor = FieldTest.__dict__['PublicStaticField']
        self.assertTrue(type(descriptor) != int)

        FieldTest.PublicStaticField = 0
        descriptor = FieldTest.__dict__['PublicStaticField']
        self.assertTrue(type(descriptor) != int)

    def test_field_descriptor_wrong_type(self):
        """Test setting a field using a value of the wrong type."""

        def test():
            FieldTest().PublicField = "spam"

        self.assertRaises(TypeError, test)

    def test_field_descriptor_abuse(self):
        """Test field descriptor abuse."""
        desc = FieldTest.__dict__['PublicField']

        def test():
            desc.__get__(0, 0)

        self.assertRaises(TypeError, test)

        def test():
            desc.__set__(0, 0)

        self.assertRaises(TypeError, test)

    def test_boolean_field(self):
        """Test boolean fields."""
        # change this to true / false later for Python 2.3?
        ob = FieldTest()
        self.assertTrue(ob.BooleanField == False)

        ob.BooleanField = True
        self.assertTrue(ob.BooleanField == True)

        ob.BooleanField = False
        self.assertTrue(ob.BooleanField == False)

        ob.BooleanField = 1
        self.assertTrue(ob.BooleanField == True)

        ob.BooleanField = 0
        self.assertTrue(ob.BooleanField == False)

    def test_sbyte_field(self):
        """Test sbyte fields."""
        ob = FieldTest()
        self.assertTrue(ob.SByteField == 0)

        ob.SByteField = 1
        self.assertTrue(ob.SByteField == 1)

    def test_byte_field(self):
        """Test byte fields."""
        ob = FieldTest()
        self.assertTrue(ob.ByteField == 0)

        ob.ByteField = 1
        self.assertTrue(ob.ByteField == 1)

    def test_char_field(self):
        """Test char fields."""
        ob = FieldTest()
        self.assertTrue(ob.CharField == u'A')
        self.assertTrue(ob.CharField == 'A')

        ob.CharField = 'B'
        self.assertTrue(ob.CharField == u'B')
        self.assertTrue(ob.CharField == 'B')

        ob.CharField = u'C'
        self.assertTrue(ob.CharField == u'C')
        self.assertTrue(ob.CharField == 'C')

    def test_int16_field(self):
        """Test int16 fields."""
        ob = FieldTest()
        self.assertTrue(ob.Int16Field == 0)

        ob.Int16Field = 1
        self.assertTrue(ob.Int16Field == 1)

    def test_int32_field(self):
        """Test int32 fields."""
        ob = FieldTest()
        self.assertTrue(ob.Int32Field == 0)

        ob.Int32Field = 1
        self.assertTrue(ob.Int32Field == 1)

    def test_int64_field(self):
        """Test int64 fields."""
        ob = FieldTest()
        self.assertTrue(ob.Int64Field == 0)

        ob.Int64Field = 1
        self.assertTrue(ob.Int64Field == 1)

    def test_uint16_field(self):
        """Test uint16 fields."""
        ob = FieldTest()
        self.assertTrue(ob.UInt16Field == 0)

        ob.UInt16Field = 1
        self.assertTrue(ob.UInt16Field == 1)

    def test_uint32_field(self):
        """Test uint32 fields."""
        ob = FieldTest()
        self.assertTrue(ob.UInt32Field == 0)

        ob.UInt32Field = 1
        self.assertTrue(ob.UInt32Field == 1)

    def test_uint64_field(self):
        """Test uint64 fields."""
        ob = FieldTest()
        self.assertTrue(ob.UInt64Field == 0)

        ob.UInt64Field = 1
        self.assertTrue(ob.UInt64Field == 1)

    def test_single_field(self):
        """Test single fields."""
        ob = FieldTest()
        self.assertTrue(ob.SingleField == 0.0)

        ob.SingleField = 1.1
        self.assertTrue(ob.SingleField == 1.1)

    def test_double_field(self):
        """Test double fields."""
        ob = FieldTest()
        self.assertTrue(ob.DoubleField == 0.0)

        ob.DoubleField = 1.1
        self.assertTrue(ob.DoubleField == 1.1)

    def test_decimal_field(self):
        """Test decimal fields."""
        ob = FieldTest()
        self.assertTrue(ob.DecimalField == System.Decimal(0))

        ob.DecimalField = System.Decimal(1)
        self.assertTrue(ob.DecimalField == System.Decimal(1))

    def test_string_field(self):
        """Test string fields."""
        ob = FieldTest()
        self.assertTrue(ob.StringField == "spam")

        ob.StringField = "eggs"
        self.assertTrue(ob.StringField == "eggs")

    def test_interface_field(self):
        """Test interface fields."""
        from Python.Test import Spam, ISpam

        ob = FieldTest()

        self.assertTrue(ISpam(ob.SpamField).GetValue() == "spam")
        self.assertTrue(ob.SpamField.GetValue() == "spam")

        ob.SpamField = Spam("eggs")
        self.assertTrue(ISpam(ob.SpamField).GetValue() == "eggs")
        self.assertTrue(ob.SpamField.GetValue() == "eggs")

    def test_object_field(self):
        """Test ob fields."""
        ob = FieldTest()
        self.assertTrue(ob.ObjectField == None)

        ob.ObjectField = System.String("spam")
        self.assertTrue(ob.ObjectField == "spam")

        ob.ObjectField = System.Int32(1)
        self.assertTrue(ob.ObjectField == 1)

        ob.ObjectField = None
        self.assertTrue(ob.ObjectField == None)

    def test_enum_field(self):
        """Test enum fields."""
        from Python.Test import ShortEnum

        ob = FieldTest()
        self.assertTrue(ob.EnumField == ShortEnum.Zero)

        ob.EnumField = ShortEnum.One
        self.assertTrue(ob.EnumField == ShortEnum.One)

    def test_nullable_field(self):
        """Test nullable fields."""
        ob = FieldTest()

        ob.StringField = None
        self.assertTrue(ob.StringField == None)

        ob.ObjectField = None
        self.assertTrue(ob.ObjectField == None)

        ob.SpamField = None
        self.assertTrue(ob.SpamField == None)

        # Primitive types and enums should not be set to null.

        def test():
            FieldTest().Int32Field = None

        self.assertRaises(TypeError, test)

        def test():
            FieldTest().EnumField = None

        self.assertRaises(TypeError, test)


def test_suite():
    return unittest.makeSuite(FieldTests)

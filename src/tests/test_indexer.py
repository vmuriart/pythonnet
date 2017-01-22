# -*- coding: utf-8 -*-

import unittest

import Python.Test as Test

from _compat import long, unichr


class IndexerTests(unittest.TestCase):
    """Test support for indexer properties."""

    def test_public_indexer(self):
        """Test public indexers."""
        ob = Test.PublicIndexerTest()

        ob[0] = "zero"
        self.assertTrue(ob[0] == "zero")

        ob[1] = "one"
        self.assertTrue(ob[1] == "one")

        self.assertTrue(ob[10] == None)

    def test_protected_indexer(self):
        """Test protected indexers."""
        ob = Test.ProtectedIndexerTest()

        ob[0] = "zero"
        self.assertTrue(ob[0] == "zero")

        ob[1] = "one"
        self.assertTrue(ob[1] == "one")

        self.assertTrue(ob[10] == None)

    def test_internal_indexer(self):
        """Test internal indexers."""
        ob = Test.InternalIndexerTest()

        with self.assertRaises(TypeError):
            ob[0] = "zero"

        with self.assertRaises(TypeError):
            Test.InternalIndexerTest.__getitem__(ob, 0)

        with self.assertRaises(TypeError):
            ob.__getitem__(0)

    def test_private_indexer(self):
        """Test private indexers."""
        ob = Test.PrivateIndexerTest()

        with self.assertRaises(TypeError):
            ob[0] = "zero"

        with self.assertRaises(TypeError):
            Test.PrivateIndexerTest.__getitem__(ob, 0)

        with self.assertRaises(TypeError):
            ob.__getitem__(0)

    def test_boolean_indexer(self):
        """Test boolean indexers."""
        ob = Test.BooleanIndexerTest()

        self.assertTrue(ob[True] == None)
        self.assertTrue(ob[1] == None)

        ob[0] = "false"
        self.assertTrue(ob[0] == "false")

        ob[1] = "true"
        self.assertTrue(ob[1] == "true")

        ob[False] = "false"
        self.assertTrue(ob[False] == "false")

        ob[True] = "true"
        self.assertTrue(ob[True] == "true")

    def test_byte_indexer(self):
        """Test byte indexers."""
        ob = Test.ByteIndexerTest()
        max = 255
        min = 0

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.ByteIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.ByteIndexerTest()
            ob["wrong"] = "wrong"

    def test_sbyte_indexer(self):
        """Test sbyte indexers."""
        ob = Test.SByteIndexerTest()
        max = 127
        min = -128

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.SByteIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.SByteIndexerTest()
            ob["wrong"] = "wrong"

    def test_char_indexer(self):
        """Test char indexers."""
        ob = Test.CharIndexerTest()
        max = unichr(65535)
        min = unichr(0)

        self.assertTrue(ob[max] == None)

        ob[max] = "max"
        self.assertTrue(ob[max] == "max")

        ob[min] = "min"
        self.assertTrue(ob[min] == "min")

        with self.assertRaises(TypeError):
            ob = Test.CharIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.CharIndexerTest()
            ob["wrong"] = "wrong"

    def test_int16_indexer(self):
        """Test Int16 indexers."""
        ob = Test.Int16IndexerTest()
        max = 32767
        min = -32768

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.Int16IndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.Int16IndexerTest()
            ob["wrong"] = "wrong"

    def test_int32_indexer(self):
        """Test Int32 indexers."""
        ob = Test.Int32IndexerTest()
        max = 2147483647
        min = -2147483648

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.Int32IndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.Int32IndexerTest()
            ob["wrong"] = "wrong"

    def test_int64_indexer(self):
        """Test Int64 indexers."""
        ob = Test.Int64IndexerTest()
        max = long(9223372036854775807)
        min = long(-9223372036854775808)

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.Int64IndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.Int64IndexerTest()
            ob["wrong"] = "wrong"

    def test_uint16_indexer(self):
        """Test UInt16 indexers."""
        ob = Test.UInt16IndexerTest()
        max = 65535
        min = 0

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.UInt16IndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.UInt16IndexerTest()
            ob["wrong"] = "wrong"

    def test_uint32_indexer(self):
        """Test UInt32 indexers."""
        ob = Test.UInt32IndexerTest()
        max = long(4294967295)
        min = 0

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.UInt32IndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.UInt32IndexerTest()
            ob["wrong"] = "wrong"

    def test_uint64_indexer(self):
        """Test UInt64 indexers."""
        ob = Test.UInt64IndexerTest()
        max = long(18446744073709551615)
        min = 0

        self.assertTrue(ob[max] == None)

        ob[max] = str(max)
        self.assertTrue(ob[max] == str(max))

        ob[min] = str(min)
        self.assertTrue(ob[min] == str(min))

        with self.assertRaises(TypeError):
            ob = Test.UInt64IndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.UInt64IndexerTest()
            ob["wrong"] = "wrong"

    def test_single_indexer(self):
        """Test Single indexers."""
        ob = Test.SingleIndexerTest()
        max = 3.402823e38
        min = -3.402823e38

        self.assertTrue(ob[max] == None)

        ob[max] = "max"
        self.assertTrue(ob[max] == "max")

        ob[min] = "min"
        self.assertTrue(ob[min] == "min")

        with self.assertRaises(TypeError):
            ob = Test.SingleIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.SingleIndexerTest()
            ob["wrong"] = "wrong"

    def test_double_indexer(self):
        """Test Double indexers."""
        ob = Test.DoubleIndexerTest()
        max = 1.7976931348623157e308
        min = -1.7976931348623157e308

        self.assertTrue(ob[max] == None)

        ob[max] = "max"
        self.assertTrue(ob[max] == "max")

        ob[min] = "min"
        self.assertTrue(ob[min] == "min")

        with self.assertRaises(TypeError):
            ob = Test.DoubleIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.DoubleIndexerTest()
            ob["wrong"] = "wrong"

    def test_decimal_indexer(self):
        """Test Decimal indexers."""
        ob = Test.DecimalIndexerTest()

        from System import Decimal
        max_d = Decimal.Parse("79228162514264337593543950335")
        min_d = Decimal.Parse("-79228162514264337593543950335")

        self.assertTrue(ob[max_d] == None)

        ob[max_d] = "max"
        self.assertTrue(ob[max_d] == "max")

        ob[min_d] = "min"
        self.assertTrue(ob[min_d] == "min")

        with self.assertRaises(TypeError):
            ob = Test.DecimalIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.DecimalIndexerTest()
            ob["wrong"] = "wrong"

    def test_string_indexer(self):
        """Test String indexers."""
        ob = Test.StringIndexerTest()

        self.assertTrue(ob["spam"] == None)
        self.assertTrue(ob[u"spam"] == None)

        ob["spam"] = "spam"
        self.assertTrue(ob["spam"] == "spam")
        self.assertTrue(ob["spam"] == u"spam")
        self.assertTrue(ob[u"spam"] == "spam")
        self.assertTrue(ob[u"spam"] == u"spam")

        ob[u"eggs"] = u"eggs"
        self.assertTrue(ob["eggs"] == "eggs")
        self.assertTrue(ob["eggs"] == u"eggs")
        self.assertTrue(ob[u"eggs"] == "eggs")
        self.assertTrue(ob[u"eggs"] == u"eggs")

        with self.assertRaises(TypeError):
            ob = Test.StringIndexerTest()
            ob[1]

        with self.assertRaises(TypeError):
            ob = Test.StringIndexerTest()
            ob[1] = "wrong"

    def test_enum_indexer(self):
        """Test enum indexers."""
        ob = Test.EnumIndexerTest()

        key = Test.ShortEnum.One

        self.assertTrue(ob[key] == None)

        ob[key] = "spam"
        self.assertTrue(ob[key] == "spam")

        ob[key] = "eggs"
        self.assertTrue(ob[key] == "eggs")

        ob[1] = "spam"
        self.assertTrue(ob[1] == "spam")

        with self.assertRaises(TypeError):
            ob = Test.EnumIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.EnumIndexerTest()
            ob["wrong"] = "wrong"

    def test_object_indexer(self):
        """Test ob indexers."""
        ob = Test.ObjectIndexerTest()

        from Python.Test import Spam
        spam = Spam("spam")

        self.assertTrue(ob[spam] == None)
        self.assertTrue(ob["spam"] == None)
        self.assertTrue(ob[1] == None)
        self.assertTrue(ob[None] == None)

        ob[spam] = "spam"
        self.assertTrue(ob[spam] == "spam")

        ob["spam"] = "eggs"
        self.assertTrue(ob["spam"] == "eggs")

        ob[1] = "one"
        self.assertTrue(ob[1] == "one")

        ob[long(1)] = "long"
        self.assertTrue(ob[long(1)] == "long")

        with self.assertRaises(TypeError):
            class eggs(object):
                pass

            key = eggs()
            ob = Test.ObjectIndexerTest()
            ob[key] = "wrong"

    def test_interface_indexer(self):
        """Test interface indexers."""
        ob = Test.InterfaceIndexerTest()

        from Python.Test import Spam
        spam = Spam("spam")

        self.assertTrue(ob[spam] == None)

        ob[spam] = "spam"
        self.assertTrue(ob[spam] == "spam")

        ob[spam] = "eggs"
        self.assertTrue(ob[spam] == "eggs")

        with self.assertRaises(TypeError):
            ob = Test.InterfaceIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.InterfaceIndexerTest()
            ob["wrong"] = "wrong"

    def test_typed_indexer(self):
        """Test typed indexers."""
        ob = Test.TypedIndexerTest()

        from Python.Test import Spam
        spam = Spam("spam")

        self.assertTrue(ob[spam] == None)

        ob[spam] = "spam"
        self.assertTrue(ob[spam] == "spam")

        ob[spam] = "eggs"
        self.assertTrue(ob[spam] == "eggs")

        with self.assertRaises(TypeError):
            ob = Test.TypedIndexerTest()
            ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.TypedIndexerTest()
            ob["wrong"] = "wrong"

    def test_multi_arg_indexer(self):
        """Test indexers that take multiple index arguments."""
        ob = Test.MultiArgIndexerTest()

        ob[0, 1] = "zero one"
        self.assertTrue(ob[0, 1] == "zero one")

        ob[1, 9] = "one nine"
        self.assertTrue(ob[1, 9] == "one nine")

        self.assertTrue(ob[10, 50] == None)

        with self.assertRaises(TypeError):
            ob = Test.MultiArgIndexerTest()
            v = ob[0, "one"]

        with self.assertRaises(TypeError):
            ob = Test.MultiArgIndexerTest()
            ob[0, "one"] = "wrong"

    def test_multi_type_indexer(self):
        """Test indexers that take multiple indices of different types."""
        ob = Test.MultiTypeIndexerTest()
        spam = Test.Spam("spam")

        ob[0, "one", spam] = "zero one spam"
        self.assertTrue(ob[0, "one", spam] == "zero one spam")

        ob[1, "nine", spam] = "one nine spam"
        self.assertTrue(ob[1, "nine", spam] == "one nine spam")

        with self.assertRaises(TypeError):
            ob = Test.MultiTypeIndexerTest()
            v = ob[0, 1, spam]

        with self.assertRaises(TypeError):
            ob = Test.MultiTypeIndexerTest()
            ob[0, 1, spam] = "wrong"

    def test_multi_default_key_indexer(self):
        """Test indexers that take multiple indices with a default key arguments."""
        # default argument is 2 in the MultiDefaultKeyIndexerTest object
        ob = Test.MultiDefaultKeyIndexerTest()
        ob[0, 2] = "zero one spam"
        self.assertTrue(ob[0] == "zero one spam")

        ob[1] = "one nine spam"
        self.assertTrue(ob[1, 2] == "one nine spam")

    def test_indexer_wrong_key_type(self):
        """Test calling an indexer using a key of the wrong type."""

        with self.assertRaises(TypeError):
            ob = Test.PublicIndexerTest()
            v = ob["wrong"]

        with self.assertRaises(TypeError):
            ob = Test.PublicIndexerTest()
            ob["wrong"] = "spam"

    def test_indexer_wrong_value_type(self):
        """Test calling an indexer using a value of the wrong type."""

        with self.assertRaises(TypeError):
            ob = Test.PublicIndexerTest()
            ob[1] = 9993.9

    def test_unbound_indexer(self):
        """Test calling an unbound indexer."""
        ob = Test.PublicIndexerTest()

        Test.PublicIndexerTest.__setitem__(ob, 0, "zero")
        self.assertTrue(ob[0] == "zero")

        Test.PublicIndexerTest.__setitem__(ob, 1, "one")
        self.assertTrue(ob[1] == "one")

        self.assertTrue(ob[10] == None)

    def test_indexer_abuse(self):
        """Test indexer abuse."""
        _class = Test.PublicIndexerTest
        ob = Test.PublicIndexerTest()

        with self.assertRaises(AttributeError):
            del _class.__getitem__

        with self.assertRaises(AttributeError):
            del ob.__getitem__

        with self.assertRaises(AttributeError):
            del _class.__setitem__

        with self.assertRaises(AttributeError):
            del ob.__setitem__


def test_suite():
    return unittest.makeSuite(IndexerTests)
